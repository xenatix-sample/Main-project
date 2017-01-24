using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Model.Setting;
using Axis.Model.Common;
using Axis.RuleEngine.Service.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Settings;
using Moq;
using Axis.Constant;

namespace Axis.RuleEngine.Tests.Controllers
{
    [TestClass]
    public class SettingsControllerTest
    {
        #region Class Variables

        private ISettingsRuleEngine settingsRuleEngine;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            Mock<ISettingsRuleEngine> mock = new Mock<ISettingsRuleEngine>();
            settingsRuleEngine = mock.Object;

            var settings = new List<SettingModel>();
            settings.Add(new SettingModel()
            {
                SettingsID = 1,
                SettingsType = SettingType.Application.ToString(),
                SettingsTypeID = 1,
                Settings = "SettingName1",
                Value = "SettingValue1",
                IsActive = true,
                ModifiedBy = 1,
                ModifiedOn = DateTime.Now
            });
            settings.Add(new SettingModel()
            {
                SettingsID = 2,
                SettingsType = SettingType.Application.ToString(),
                SettingsTypeID = 1,
                Settings = "SettingName2",
                Value = "SettingValue2",
                IsActive = true,
                ModifiedBy = 1,
                ModifiedOn = DateTime.Now
            });

            var allSettings = new Response<SettingModel>()
            {
                DataItems = settings
            };

            //GetNonUserSettings
            Response<SettingModel> settingResponse = new Response<SettingModel>();
            settingResponse.DataItems = settings.Where(s => s.Value.Contains("SettingValue")).ToList();
            mock.Setup(s => s.GetNonUserSettings()).Returns(allSettings);

            //UpdateSetting
            mock.Setup(s => s.UpdateSetting(It.IsAny<SettingModel>())).Callback((SettingModel settingModel) => settings.Add(settingModel)).Returns(allSettings);

            //GetSettingsToCache
            Response<SettingModel> settingCacheResponse = new Response<SettingModel>();
            settingCacheResponse.DataItems = settings.Where(s => s.Value.Contains("SettingValue")).ToList();
            mock.Setup(s => s.GetSettingsToCache(It.IsAny<bool>())).Returns(allSettings);
        }

        [TestMethod]
        public void GetNonUserSettings()
        {
            SettingsController settingsController = new SettingsController(settingsRuleEngine);
            var getNonUserSettingsResult = settingsController.GetNonUserSettings();
            var response = getNonUserSettingsResult as HttpResult<Response<SettingModel>>;
            var settingsList = response.Value.DataItems;

            Assert.IsNotNull(settingsList);
            Assert.IsTrue(settingsList.Count > 0);
        }

        [TestMethod]
        public void UpdateSetting()
        {
            SettingsController settingsController = new SettingsController(settingsRuleEngine);
            SettingModel updateModel = new SettingModel() {  };
            var updateSettingResult = settingsController.UpdateSetting(updateModel);
            var response = updateSettingResult as HttpResult<Response<SettingModel>>;
            var data = response.Value.DataItems;

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Contains(updateModel));
            Assert.AreEqual(3, data.Count());
        }

        [TestMethod]
        public void GetSettingsToCache()
        {
            SettingsController settingsController = new SettingsController(settingsRuleEngine);
            var getSettingsToCacheResult = settingsController.GetSettingsToCache(false);
            var response = getSettingsToCacheResult as HttpResult<Response<SettingModel>>;
            var settingsList = response.Value.DataItems;

            Assert.IsNotNull(settingsList);
            Assert.IsTrue(settingsList.Count > 0);
        }

        #endregion
    }
}
