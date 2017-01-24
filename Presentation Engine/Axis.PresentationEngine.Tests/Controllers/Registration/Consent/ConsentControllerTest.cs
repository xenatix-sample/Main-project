using System.Configuration;
using System.Web.Mvc;
using Axis.Plugins.Registration.ApiControllers;
using MvcControllers = Axis.Plugins.Registration.Controllers;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axis.PresentationEngine.Tests.Controllers.Registration.Consent
{
    [TestClass]
    public class ConsentControllerTest
    {
        #region Class Variables

        private ConsentController _controller;
        //private MvcControllers.ConsentController _mvcController;
        private long _defaultContactId = 1;
        private string _sigImage = "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";

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            _controller = new ConsentController(new ConsentRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        #region Action Reults

        [TestMethod]
        public void Index_Success()
        {
            //ActionResult result = _mvcController.Index();

            //Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexInstance_Success()
        {
            //ActionResult result = _mvcController.Index();

            //Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        #endregion

        #region Json Results

        [TestMethod]
        public void AddConsentSignature_Success()
        {
            var modelResponse = _controller.AddConsentSignature(new ConsentViewModel() { SignatureBlob = _sigImage, ContactId = _defaultContactId, IsActive = true, ModifiedBy = 3, ForceRollback = true });
            var rowsAffected = modelResponse.RowAffected;

            Assert.IsTrue(rowsAffected > 0);
        }

        [TestMethod]
        public void AddConsentSignature_Failure()
        {
            var modelResponse = _controller.AddConsentSignature(new ConsentViewModel() { SignatureBlob = _sigImage, IsActive = true, ContactId = -1, ModifiedBy = 3, ForceRollback = true });
            var resultCode = modelResponse.ResultCode;

            Assert.IsTrue(resultCode != 0);
        }

        [TestMethod]
        public void GetConsentSignature_Success()
        {
            var modelResponse = _controller.GetConsentSignature(_defaultContactId).Result;
            var count = modelResponse.DataItems.Count;

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void GetConsentSignature_Failure()
        {
            var modelResponse = _controller.GetConsentSignature(-1).Result;
            var resultCode = modelResponse.ResultCode;

            Assert.IsTrue(resultCode != 0);
        }

        #endregion

        #endregion
    }
}
