using System.Collections.Generic;
using System.Web.Http;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Settings;
using Axis.Model.Common;
using Axis.Constant;
using Axis.Helpers.Caching;
using Axis.Model.Setting;
using Axis.DataEngine.Helpers.Controllers;

namespace Axis.DataEngine.Service.Controllers
{

    public class SettingsController : BaseApiController
    {
        #region Class Variables

        ISettingsDataProvider settingsDataProvider = null;

        #endregion

        #region Constructors

        public SettingsController(ISettingsDataProvider settingsDataProvider)
        {
            this.settingsDataProvider = settingsDataProvider;
        }

        #endregion

        #region Public Methods

        [HttpGet]
        public IHttpActionResult GetNonUserSettings()
        {
            List<SettingType> settingTypes = new List<SettingType>() { SettingType.Application, SettingType.Plugin};
            return new HttpResult<Response<SettingModel>>(settingsDataProvider.GetSettingsByType(settingTypes), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateSetting(SettingModel setting)
        {
            return new HttpResult<Response<SettingModel>>(settingsDataProvider.UpdateSetting(setting), Request);
        }

        [HttpGet]
        public IHttpActionResult GetSettingsToCache(bool forceServerCacheReset)
        {
            var response = settingsDataProvider.GetSettingsToCache();

            if (response.ResultCode == 0)
            {
                //Cache the settings retrieved from the database for this layer
                SettingsCacheManager settingsCache = new SettingsCacheManager(response.DataItems, forceServerCacheReset);
            }

            return new HttpResult<Response<SettingModel>>(response, Request);
        }

        #endregion
    }
}