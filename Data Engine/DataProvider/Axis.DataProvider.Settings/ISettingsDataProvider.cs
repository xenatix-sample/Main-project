using System.Collections.Generic;
using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Setting;

namespace Axis.DataProvider.Settings
{
    public interface ISettingsDataProvider
    {
        Response<SettingModel> GetSettingsByType(List<SettingType> settingTypes);
        Response<SettingModel> UpdateSetting(SettingModel setting);
        Response<SettingModel> GetSettingsToCache();
    }
}
