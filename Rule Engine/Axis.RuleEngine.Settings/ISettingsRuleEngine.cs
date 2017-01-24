using Axis.Model.Common;
using Axis.Model.Setting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Settings
{
    public interface ISettingsRuleEngine
    {
        Response<SettingModel> GetNonUserSettings();
        Response<SettingModel> UpdateSetting(SettingModel setting);
        Response<SettingModel> GetSettingsToCache(bool forceServerCacheReset);
    }
}
