using System;
using System.Collections.Specialized;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Axis.Helpers.Caching;
using Axis.Logging;
using Axis.Helpers.Infrastructure;
using Axis.Model.Common;
using Axis.Model.Setting;
using Axis.Service;
using Axis.PresentationEngine.App_Start;
using Microsoft.ApplicationInsights.Extensibility;
using System.Configuration;

namespace Axis.PresentationEngine
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            GlobalConfiguration.Configure(WebApiConfig.Register);
            EngineContext.Initialize(false);

            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            JsonConfig.Setup();

            Config.ConfigurationLog(Server.MapPath("~/Web.config"));
            SetServerSettings();

            bool _enableAppInsights = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableAppInsights"));
            if(_enableAppInsights)
            {
                TelemetryConfiguration.Active.InstrumentationKey = ConfigurationManager.AppSettings.Get("AppInsightsKey");
            }
            else
            {
                TelemetryConfiguration.Active.DisableTelemetry = true;
            }


        }

        #region Private Methods

        private static async void SetServerSettings()
        {
            Response<SettingModel> responseObj;

            var communicationManager = new CommunicationManager();
            const string baseRoute = "settings/";
            const string apiUrl = baseRoute + "GetSettingsToCacheOnStart";
            var param = new NameValueCollection();
            param.Add("forceServerCacheReset", "false");

            try
            {
                responseObj = await communicationManager.GetAsync<Response<SettingModel>>(param, apiUrl);

                if (responseObj != null)
                {
                    if (responseObj.ResultCode == 0)
                    {
                        //Cache the settings retrieved from the database for this layer
                        SettingsCacheManager settingsCache = new SettingsCacheManager(responseObj.DataItems, false);
                    }
                }
            }
            catch (Exception ex)
            {
                //Rule/Data-engine is not available. Do not prevent the application from starting
                var logger = new Logger(true);
                logger.Error(ex);
            }
        }

        #endregion
    }
}
