using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Clinical.ApiControllers;
using System.Configuration;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Repository.PresentIllness;
using Axis.Plugins.Clinical.Models.PresentIllness;

namespace Axis.PresentationEngine.Tests.Controllers.Clinical.PresentIllness
{
    [TestClass]
    public class PresentIllnessDetailsLiveTest
    {
        /// <summary>
        /// The default contact identifier
        /// </summary>
        //private int defaultContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private PresentIllnessController controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new PresentIllnessController(new PresentIllnessRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        
        /// </summary>
        [TestMethod]
        public void AddHPIDetail_Success()
        {
            //Arrange
            var srModel = new PresentIllnessDetailViewModel
            {
                HPIDetailID = 57,
                HPIID = 1,
                Comment = "Some comment",
                Location = "SOme location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.AddHPIDetail(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID > 0, "HPI Detail could not be created.");
        }

        /// <summary>
        /// Adds the hpi details failure.
        /// </summary>
        [TestMethod]
        public void AddHPIDetail_Failure()
        {
            //Arrange
            var srModel = new PresentIllnessDetailViewModel
            {
                HPIDetailID = -57,
                HPIID = 1,
                Comment = "Some comment",
                Location = "SOme location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.AddHPIDetail(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID <= 0, "HPI created for invalid data.");
        }

        /// <summary>
        /// Updates the HPI  details success.
        /// </summary>
        [TestMethod]
        public void UpdateHPIDetail_Success()
        {
            //Arrange
            var srModel = new PresentIllnessDetailViewModel
            {
                HPIDetailID = 57,
                HPIID = 1,
                Comment = "Some reasonable comment",
                Location = "SOme different location",
                Quality = "Living with Family.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.UpdateHPIDetail(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "HPI could not be updated.");
        }

        /// <summary>
        /// Updates the HPI details failure.
        /// </summary>
        [TestMethod]
        public void UpdateHPIDetail_Failure()
        {
            //Arrange
            var srModel = new PresentIllnessDetailViewModel
            {
                HPIDetailID = -57,
                HPIID = 1,
                Comment = "Some reasonable comment",
                Location = "SOme different location",
                Quality = "SOme QUality Updated.",
                HPISeverityID = 1,
                Duration = " Some duration",
                Timing = "Some  timing",
                Context = "Some context",
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.UpdateHPIDetail(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Present illness updated for invalid data.");
        }

        /// <summary>
        /// Deletes the hpi details success.
        /// </summary>
        [TestMethod]
        public void DeleteHPIDetail_Success()
        {
            // Act
            var response = controller.DeleteHPIDetail(defaultDeleteID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "hpi detail could not be deleted.");
        }

        /// <summary>
        /// Deletes the SocialRelationship details failure.
        /// </summary>
        [TestMethod]
        public void DeleteHPIDetail_Failure()
        {
            // Act
            var response = controller.DeleteHPIDetail(9090, DateTime.UtcNow);      //some invalid ID

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Present Illness deleted for invalid data.");
        }

       
    }
}
