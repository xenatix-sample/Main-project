using System;
using System.Configuration;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Helpers;
using Axis.Model.Common;
using Axis.Model.Setting;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axis.DataEngine.Tests.Controllers.Settings
{
    [TestClass]
    public class SettingsControllerLiveTest
    {
        #region Class Variables

        private HttpClient httpClient;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["UnitTestUrl"]);
            httpClient.DefaultRequestHeaders.Add("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);    
        }

        [TestCleanup]
        public void TearDown()
        {
            httpClient.Dispose();
        }

        [TestMethod]
        public async Task GetNonUserSettings()
        {
            var response = await httpClient.GetAsync("/api/Settings/GetNonUserSettings");
            var result = response.Content.ReadAsStringAsync().Result;
            var model = Json.Decode<Response<SettingModel>>(result);

            Assert.IsTrue(model.DataItems.Count > 0);
        }

        [TestMethod]
        public async Task UpdateSetting()
        {
            var settingModel = new SettingModel() { SettingsID = 1, Settings = "Theme", Value = "Dark", SettingsTypeID = 1, SettingsType = "Application", EntityID = null, IsActive = true, ModifiedBy = 1, ModifiedOn = DateTime.Now };
            var response =
                await httpClient.PostAsync<SettingModel>("/api/Settings/UpdateSetting", settingModel, new JsonMediaTypeFormatter());
            var result = response.Content.ReadAsStringAsync().Result;
            var responseModel = Json.Decode<Response<SettingModel>>(result);
            Assert.IsTrue(responseModel.ResultCode == 0, "Result Code must be 0 (Success)");
            Assert.IsTrue(responseModel.RowAffected == 1, "Rows Affected must equal 1");
        }

        [TestMethod]
        public async Task GetSettingsToCache()
        {
            var response = await httpClient.GetAsync("/api/Settings/GetSettingsToCache?forceServerCacheReset=false");
            var result = response.Content.ReadAsStringAsync().Result;
            var model = Json.Decode<Response<SettingModel>>(result);

            Assert.IsTrue(model.DataItems.Count > 0);
        }

        #endregion
    }
}
