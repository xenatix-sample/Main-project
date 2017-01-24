using Axis.Model.Common;
using Axis.Model.Setting;

namespace Axis.Service.Settings
{
    public interface ISettingsService
    {
        Response<SettingModel> GetNonUserSettings();
        Response<SettingModel> UpdateSetting(SettingModel setting);
        Response<SettingModel> GetSettingsToCache(bool forceServerCacheReset);
    }
}
