using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.ECI.ApiControllers;
using Axis.Plugins.ECI;
using System.Configuration;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Model;

namespace Axis.PresentationEngine.Tests.Controllers.ECI.IFSP
{
    [TestClass]
    public class IFSPTest
    {
        /// <summary>
        /// The default contact identifier
        /// </summary>
        private int defaultContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteIFSPID = 7;

        /// <summary>
        /// The controller
        /// </summary>
        private IFSPController controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new IFSPController(new IFSPRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the IFSP_success.
        /// </summary>
        [TestMethod]
        public void GetIFSP_Success()
        {
            // Act
            var response = controller.GetIFSPList(defaultContactId).Result;

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one IFSP must exists.");
        }

        /// <summary>
        /// Gets the IFSP_failure.
        /// </summary>
        [TestMethod]
        public void GetIFSP_Failure()
        {
            // Act
            var response = controller.GetIFSPList(-1).Result;

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "IFSP exists for invalid data.");
        }

        /// <summary>
        /// Adds the IFSP_ success.
        /// </summary>
        [TestMethod]
        public void AddIFSP_Success()
        {
            // Act
            var ifspModel = new IFSPDetailViewModel
            {
                IFSPID = 0,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            var response = controller.AddIFSP(ifspModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "IFSP could not be created.");
        }

        /// <summary>
        /// Adds the IFSP_ failure.
        /// </summary>
        [TestMethod]
        public void AddIFSP_Failure()
        {
            // Act
            var ifspModel = new IFSPDetailViewModel
            {
                IFSPID = 0,
                ContactID = -1,
                IFSPTypeID = -1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Failure test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            var response = controller.AddIFSP(ifspModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "IFSP created for invalid data.");
        }

        /// <summary>
        /// Updates the IFSP_ success.
        /// </summary>
        [TestMethod]
        public void UpdateIFSP_Success()
        {
            // Act
            var ifspModel = new IFSPDetailViewModel
            {
                IFSPID = 1,
                ContactID = 1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            var response = controller.UpdateIFSP(ifspModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "IFSP could not be updated.");
        }

        /// <summary>
        /// Updates the IFSP_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateIFSP_Failure()
        {
            // Act
            var ifspModel = new IFSPDetailViewModel
            {
                IFSPID = -1,
                ContactID = -1,
                IFSPTypeID = 1,
                IFSPMeetingDate = DateTime.Now,
                IFSPFamilySignedDate = DateTime.Now,
                MeetingDelayed = true,
                ReasonForDelayID = 1,
                Comments = "Success test case",
                AssessmentID = 1,
                ResponseID = 1,
                ForceRollback = true
            };

            var response = controller.UpdateIFSP(ifspModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "IFSP updated for invalid data.");
        }
        
        /// <summary>
        /// Deletes the IFSP_ success.
        /// </summary>
        [TestMethod]
        public void DeleteIFSP_Success()
        {
            // Act
            var response = controller.RemoveIFSP(defaultDeleteIFSPID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "IFSP could not be deleted.");
        }

        /// <summary>
        /// Deletes the collateral_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteIFSP_Failure()
        {
            // Act
            var response = controller.RemoveIFSP(-1, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "IFSP deleted for invalid data.");
        }
    }
}
