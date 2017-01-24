using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Net.Http;
using System.Threading.Tasks;
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
using Microsoft.ApplicationInsights.Extensibility;

namespace Axis.DataEngine.Service
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            EngineContext.Initialize(false);

            AppDomain.CurrentDomain.UnhandledException += UnhandledExceptionLogger;

            AreaRegistration.RegisterAllAreas();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            Config.ConfigurationLog(Server.MapPath("~/Web.config"));
            SetServerSettings();

            bool _enableAppInsights = Convert.ToBoolean(ConfigurationManager.AppSettings.Get("EnableAppInsights"));
            if (_enableAppInsights)
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
            var communicationManager = new CommunicationManager();
            const string baseRoute = "settings/";
            const string apiUrl = baseRoute + "GetSettingsToCache";
            var param = new NameValueCollection();
            param.Add("forceServerCacheReset", "false");

            //Set the items in the cache in an ansync fashion
            await communicationManager.GetAsync<Response<SettingModel>>(param, apiUrl);
        }

        private static void UnhandledExceptionLogger(object sender, UnhandledExceptionEventArgs e)
        {
            var logger = new Logger(true);
            logger.Error((Exception)e.ExceptionObject);
        }

        #endregion
    }
}
