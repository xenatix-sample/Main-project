using System.Configuration;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Configuration.Controllers;
using Axis.PresentationEngine.Areas.Configuration.Models;
using Axis.PresentationEngine.Areas.Configuration.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Web.Mvc;

namespace Axis.PresentationEngine.Tests.Controllers
{
    [TestClass]
    public class SettingsControllerTest
    {
        #region Class Variables
                
        private SettingsController _controller;

        #endregion

        #region Initialization

        [TestInitialize]
        public void Initialize()
        {
            _controller = new SettingsController(new ConfigurationRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        #endregion

        #region Action Results

        [TestMethod]
        public void Index_Success()
        {
            ActionResult result = _controller.Index();
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void Edit_Success()
        {
            ActionResult result = _controller.Edit();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void GetSettingsToCache_Success()
        {
            ActionResult result = _controller.GetSettingsToCache();

            Assert.IsNotNull(result);
        }

        #endregion

        #region Json Results

        [TestMethod]
        public void GetNonUserSettings_Success()
        {
            var result = _controller.GetNonUserSettings();
            var data = result.Data;
            var modelResponse = (Response<SettingsViewModel>)data;
            var count = modelResponse.DataItems.Count;

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void UpdateSetting_Success()
        {
            var result = _controller.UpdateSetting(new SettingsViewModel() { SettingID = 1, SettingValue = "Dark", SettingName = "Theme", ForceRollback = true});
            var data = result.Data;
            var modelResponse = (Response<SettingsViewModel>)data;
            var rowsAffected = modelResponse.RowAffected;

            Assert.IsTrue(rowsAffected > 0);
        }

        #endregion
    }
}
