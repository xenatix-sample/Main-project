using Axis.Model.Common;
using Axis.Model.Setting;
using Axis.Service.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Settings
{
    public class SettingsRuleEngine : ISettingsRuleEngine
    {
        #region Class Variables

        private ISettingsService settingsService;

        #endregion

        #region Constructors

        public SettingsRuleEngine(ISettingsService settingsService)
        {
            this.settingsService = settingsService;
        }

        #endregion

        #region Public Methods

        public Response<SettingModel> GetNonUserSettings()
        {
            return settingsService.GetNonUserSettings();
        }

        public Response<SettingModel> UpdateSetting(SettingModel setting)
        {
            return settingsService.UpdateSetting(setting);
        }

        public Response<SettingModel> GetSettingsToCache(bool forceServerCacheReset)
        {
            return settingsService.GetSettingsToCache(forceServerCacheReset);
        }

        #endregion
    }
}
