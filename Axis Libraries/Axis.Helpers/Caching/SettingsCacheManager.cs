using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Caching;
using System.Web.Mvc.Filters;
using Axis.Model.Setting;
using Axis.Constant;

namespace Axis.Helpers.Caching
{
    public class SettingsCacheManager : MemoryCacheManager
    {
        #region Class Variables

        private const string Key = "Settings";
        //private const int CacheTime = 20;

        #endregion

        #region Constructors

        public SettingsCacheManager()
        {

        }

        public SettingsCacheManager(List<SettingModel> data)
        {
            Set(data, false);
        }

        public SettingsCacheManager(List<SettingModel> data, bool forceRefresh = false)
        {
            Set(data, forceRefresh);
        }

        #endregion

        #region Public Methods

        public void Set(List<SettingModel> data, bool forceRefresh)
        {
            if (data == null || data.Count == 0)
                return;

            List<SettingModel> tmpData = data.ToList();

            //The cache is already set and we are not forcing a refresh
            if (IsSet(Key) && !forceRefresh)
                return;

            //User settings are not needed in the in-memory cache
            tmpData.RemoveAll(s => s.SettingsTypeID == (int)SettingType.User);

            var policy = new CacheItemPolicy
            {
                //AbsoluteExpiration = DateTime.Now + TimeSpan.FromMinutes(CacheTime),
                RemovedCallback = ItemRemoved
            };
            Cache.Set(new CacheItem(Key, tmpData), policy);
        }

        public List<SettingModel> GetAllAppSettings()
        {
            var settingsList = Get<List<SettingModel>>(Key);
            if (settingsList == null)
                return null;

            return settingsList.Where(s => s.SettingsTypeID == (int)SettingType.Application).ToList();
        }

        public List<SettingModel> GetAllPluginSettings()
        {
            var settingsList = Get<List<SettingModel>>(Key);
            if (settingsList == null)
                return null;

            return settingsList.Where(s => s.SettingsTypeID == (int)SettingType.Plugin).ToList();
        }

        public SettingModel GetSettingByEnum(Setting settingEnum)
        {
            var settingsList = Get<List<SettingModel>>(Key);
            if (settingsList == null)
                return null;

            //Take the highest ranking settingtype based on ID
            SettingModel setting = settingsList.OrderBy(s => s.SettingsTypeID).Reverse().FirstOrDefault(x => x.SettingsID == (int)settingEnum);
            return setting;
        }

        public List<SettingModel> GetAppSettingsByEnums(List<Setting> settings)
        {
            var settingsList = Get<List<SettingModel>>(Key);
            if (settingsList == null)
                return null;

            var settingIdList = settings.Select(tmpSetting => (int) tmpSetting).ToList();

            var matchedSettings =
                settingsList.Where(s => s.SettingsTypeID == (int) SettingType.Application && settingIdList.Contains(s.SettingsID)).ToList();

            return matchedSettings;
        }

        public SettingModel GetAppSettingByEnum(Setting settingEnum)
        {
            var settingsList = Get<List<SettingModel>>(Key);
            if (settingsList == null)
                return null;

            SettingModel setting = settingsList.FirstOrDefault(x => x.SettingsID == (int)settingEnum && x.SettingsTypeID == (int)SettingType.Application);
            return setting;
        }
        
        public SettingModel GetPluginSettingByEnum(Setting settingEnum, Plugins plugin)
        {
            var settingsList = Get<List<SettingModel>>(Key);
            if (settingsList == null)
                return null;

            SettingModel setting = settingsList.FirstOrDefault(x => x.SettingsID == (int)settingEnum && x.SettingsTypeID == (int)SettingType.Plugin && x.EntityID == (int)plugin);
            return setting;
        }

        public void RefreshSettings()
        {
            //refresh the settings cache
        }

        #endregion

        #region Private Methods

        private void ItemRemoved(CacheEntryRemovedArguments arguments)
        {
            //refresh the settings cache when expired
        }

        #endregion
    }
}
