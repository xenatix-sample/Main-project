using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Axis.Model.Common;
using Axis.Model.Setting;
using Axis.PresentationEngine.Areas.Configuration.Models;

namespace Axis.PresentationEngine.Areas.Configuration.Repository
{
    public interface IConfigurationRepository
    {
        Response<SettingsViewModel> GetNonUserSettings();
        Response<SettingsViewModel> UpdateSetting(SettingsViewModel setting);
        Response<SettingsViewModel> GetSettingsToCache(bool forceServerCacheReset);
    }
}