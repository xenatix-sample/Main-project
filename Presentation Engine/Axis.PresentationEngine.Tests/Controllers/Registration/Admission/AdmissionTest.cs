using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Registration.ApiControllers;
using Axis.Plugins.Registration.Repository;
using System.Web.Mvc;
using Axis.Plugins.Registration.Model;
using Axis.Model.Common;
using System.Configuration;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Repository.Admission;


namespace Axis.PresentationEngine.Tests.Controllers
{
    [TestClass]
    public class AdmissionTest
    {

        #region Class Variables

        /// <summary>
        /// The communication manager
        /// </summary>
        private AdmissionController controller = null;

        /// <summary>
        /// The request model
        /// </summary>
        private AdmissionViewModal requestModel = null;

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
            // Arrange
            controller = new AdmissionController(new AdmissionRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
            requestModel = new AdmissionViewModal
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
            // Act
            var response = controller.GetAdmission(contactID);

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
            // Act
            var response = controller.GetAdmission(-1);

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
            // Act
            var response = controller.AddAdmission(requestModel);

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
            requestModel.ContactAdmissionID = -1;
            requestModel.ContactID = -1;

            // Act
            // Act
            var response = controller.AddAdmission(requestModel);

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
            requestModel.ContactAdmissionID = 1;
            requestModel.Comments = "update test";

            // Act
            var response = controller.UpdateAdmission(requestModel);

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
            requestModel.ContactAdmissionID = -1;
            requestModel.Comments = "update test";

            // Act
            var response = controller.UpdateAdmission(requestModel);

            // Assert
            Assert.IsNotNull(response, "Response can't be null.");
            Assert.IsTrue(response.RowAffected == 0, "admission updated for invalid data.");
        }
    }
}
