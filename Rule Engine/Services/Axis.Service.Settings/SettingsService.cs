using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Model.Setting;
using Axis.Security;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Axis.Service.Settings
{
    public class SettingsService : ISettingsService
    {
        #region Class Variables

        private CommunicationManager communicationManager;
        private const string baseRoute = "settings/";

        #endregion

        #region Constructors

        public SettingsService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<SettingModel> GetNonUserSettings()
        {
            var apiUrl = baseRoute + "GetNonUserSettings";
            return communicationManager.Get<Response<SettingModel>>(apiUrl);
        }

        public Response<SettingModel> UpdateSetting(SettingModel setting)
        {
            var apiUrl = baseRoute + "UpdateSetting";
            return communicationManager.Post<SettingModel, Response<SettingModel>>(setting, apiUrl);
        }

        public Response<SettingModel> GetSettingsToCache(bool forceServerCacheReset)
        {
            var apiUrl = baseRoute + "GetSettingsToCache";
            var param = new NameValueCollection();
            param.Add("forceServerCacheReset", forceServerCacheReset.ToString());

            var response = communicationManager.Get<Response<SettingModel>>(param, apiUrl);
            return response;
        }

        #endregion
    }
}
