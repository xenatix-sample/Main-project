using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Setting;
using Axis.PresentationEngine.Areas.Configuration.Models;
using Axis.PresentationEngine.Areas.Configuration.Translator;
using Axis.Service;
using System.Collections.Specialized;

namespace Axis.PresentationEngine.Areas.Configuration.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class ConfigurationRepository : IConfigurationRepository
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "settings/";

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRepository"/> class.
        /// </summary>
        public ConfigurationRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ConfigurationRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructors

        #region Methods

        /// <summary>
        /// Gets the non user settings.
        /// </summary>
        /// <returns></returns>
        
        public Response<SettingsViewModel> GetNonUserSettings()
        {
            string apiUrl = baseRoute + "GetNonUserSettings";
            var response = communicationManager.Get<Response<SettingModel>>(apiUrl);

            return response.ToModel();
        }

        /// <summary>
        /// Updates the setting.
        /// </summary>
        /// <param name="setting">The setting.</param>
        /// <returns></returns>
      
        public Response<SettingsViewModel> UpdateSetting(SettingsViewModel setting)
        {
            string apiUrl = baseRoute + "UpdateSetting";
            var response = communicationManager.Post<SettingModel, Response<SettingModel>>(setting.ToModel(), apiUrl);

            return response.ToModel();
        }

        /// <summary>
        /// Gets the settings to cache.
        /// </summary>
        /// <param name="forceServerCacheReset">if set to <c>true</c> [force server cache reset].</param>
        /// <returns></returns>        
        public Response<SettingsViewModel> GetSettingsToCache(bool forceServerCacheReset)
        {
            string apiUrl = baseRoute + "GetSettingsToCache";
            var param = new NameValueCollection();
            param.Add("forceServerCacheReset", forceServerCacheReset.ToString());

            var response = communicationManager.Get<Response<SettingModel>>(param, apiUrl);

            return response.ToModel();
        }

        #endregion Methods
    }
}