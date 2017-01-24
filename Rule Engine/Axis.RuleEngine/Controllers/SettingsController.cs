using Axis.Constant;
using Axis.Helpers.Caching;
using Axis.Model.Common;
using Axis.Model.Setting;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Settings;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    
    public class SettingsController : ApiController
    {
        #region Class Variables

        ISettingsRuleEngine settingsRuleEngine = null;

        #endregion

        #region Constructors

        public SettingsController(ISettingsRuleEngine settingsRuleEngine)
        {
            this.settingsRuleEngine = settingsRuleEngine;
        }

        #endregion

        #region Public Methods
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_Settings_Settings, Permission = Permission.Read)]
        public IHttpActionResult GetNonUserSettings()
        {
            Response<SettingModel> data = settingsRuleEngine.GetNonUserSettings();

            return new HttpResult<Response<SettingModel>>(data, Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_Settings_Settings, Permission = Permission.Update)]
        public IHttpActionResult UpdateSetting(SettingModel setting)
        {
            Response<SettingModel> data = settingsRuleEngine.UpdateSetting(setting);

            return new HttpResult<Response<SettingModel>>(data, Request);
        }

        public IHttpActionResult GetSettingsToCache(bool forceServerCacheReset)
        {
            Response<SettingModel> data = settingsRuleEngine.GetSettingsToCache(forceServerCacheReset);
            
            if(data.ResultCode == 0)
            {
                //Cache the settings retrieved from the database
                SettingsCacheManager settingsCache = new SettingsCacheManager(data.DataItems, forceServerCacheReset);
            }

            return new HttpResult<Response<SettingModel>>(data, Request);
        }

        public IHttpActionResult GetSettingsToCacheOnStart(bool forceServerCacheReset)
        {
            Response<SettingModel> data = settingsRuleEngine.GetSettingsToCache(forceServerCacheReset);

            if (data.ResultCode == 0)
            {
                //Cache the settings retrieved from the database
                SettingsCacheManager settingsCache = new SettingsCacheManager(data.DataItems, forceServerCacheReset);
            }

            return new HttpResult<Response<SettingModel>>(data, Request);
        }

        #endregion
    }
}