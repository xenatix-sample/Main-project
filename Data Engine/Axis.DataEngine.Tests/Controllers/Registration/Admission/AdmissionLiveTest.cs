using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers
{
    [TestClass]
    public class AdmissionLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Admission/";


        /// <summary>
        /// The request model
        /// </summary>
        private AdmissionModal requestModel = null;

        /// <summary>
        /// contactId
        /// </summary>
        private long contactID = 1;

        #endregion Class Variables

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
            requestModel = new AdmissionModal
            {
                ContactID = 1,
                ProgramUnitID = 1,
                EffectiveDate = DateTime.Now,
                UserID = 5,
                IsDocumentationComplete = false,
                Comments = "Test",
                ForceRollback = true
            };
        }

        /// <summary>
        /// Get admission success
        /// </summary>
        [TestMethod]
        public void GetAdmission_Success()
        {
            // Arrange
            var url = baseRoute + "GetAdmission";
            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            var response = communicationManager.Get<Response<AdmissionModal>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one admission must exists.");
        }

        /// <summary>
        /// Get admission failed
        /// </summary>
        [TestMethod]
        public void GetAdmission_Failed()
        {
            // Arrange
            var url = baseRoute + "GetAdmission";
            var param = new NameValueCollection();
            param.Add("contactID", "-1");

            var response = communicationManager.Get<Response<AdmissionModal>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            
        }

        /// <summary>
        /// Add admission success unit test.
        /// </summary>
        [TestMethod]
        public void AddAdmission_Success()
        {
            // Arrange
            var url = baseRoute + "AddAdmission";           

            // Act
            var response = communicationManager.Post<AdmissionModal, Response<AdmissionModal>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0 && response.RowAffected > 0, "admission could not be created");
        }

        /// <summary>
        /// Add admission failed unit test.
        /// </summary>
        [TestMethod]
        public void AddAdmission_Failed()
        {
            // Arrange
            var url = baseRoute + "AddAdmission";
            requestModel.ContactAdmissionID = -1;
            requestModel.ContactID = -1;

            // Act
            var response = communicationManager.Post<AdmissionModal, Response<AdmissionModal>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "admission created for invalid data");
        }

        /// <summary>
        /// update admission success unit test.
        /// </summary>
        [TestMethod]
        public void UpdateAdmission_Success()
        {
            // Arrange
            var url = baseRoute + "UpdateAdmission";
            requestModel.ContactAdmissionID = 1;
            requestModel.Comments = "update test";

            // Act
            var response = communicationManager.Put<AdmissionModal, Response<AdmissionModal>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0 && response.RowAffected > 0, "admission could not be updated.");
        }

        /// <summary>
        /// update admission Failed unit test.
        /// </summary>
        [TestMethod]
        public void UpdateAdmission_Failed()
        {
            // Arrange
            var url = baseRoute + "UpdateAdmission";
            requestModel.ContactAdmissionID = -1;
            requestModel.Comments = "update test";
            // Act
            var response = communicationManager.Put<AdmissionModal, Response<AdmissionModal>>(requestModel, url);

            // Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "admission updated for invalid data.");
        }
    }
}