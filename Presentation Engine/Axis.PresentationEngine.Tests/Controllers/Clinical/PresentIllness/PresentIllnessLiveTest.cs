using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Clinical.Repository.PresentIllness;
using Axis.Plugins.Clinical.ApiControllers;
using System.Configuration;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.PresentIllness;

namespace Axis.PresentationEngine.Tests.Controllers.Clinical.PresentIllness
{
    [TestClass]
    public class PresentIllnessLiveTest
    {
        /// <summary>
        /// The default contact identifier
        /// </summary>
        private int defaultContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteID = 10003;

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

        /// <summary>
        /// Gets the GetHPI_success.
        /// </summary>
        [TestMethod]
        public void GetHPI_Success()
        {
            // Act
            var response = controller.GetHPI(defaultContactId);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Present Illness must exist.");
        }

        /// <summary>
        /// Gets the GetHPI_failure.
        /// </summary>
        [TestMethod]
        public void GetHPI_Failure()
        {
            // Act
            var response = controller.GetHPI(-1);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Present Illness exists for invalid data.");
        }

        /// <summary>
        /// Adds the Present Illness_ success.
        /// </summary>
        [TestMethod]
        public void AddHPI_Success()
        {
            // Act
            var srModel = new PresentIllnessViewModel
            {
                HPIID = 0,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddHPI(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID > 0, "Present  Illness could not be created.");
        }

        /// <summary>
        /// Adds the PresentIllness_ failure.
        /// </summary>
        [TestMethod]
        public void AddHPI_Failure()
        {
            // Act
            var srModel = new PresentIllnessViewModel
            {
                HPIID = 0,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddHPI(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID <= 0, "Present  Illness created for invalid data.");
        }

        /// <summary>
        /// Updates the HPI_ success.
        /// </summary>
        [TestMethod]
        public void UpdateHPI_Success()
        {
            // Act
            var srModel = new PresentIllnessViewModel
            {
                HPIID = 1,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateHPI(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Present  Illness could not be updated.");
        }

        /// <summary>
        /// Updates the hpi_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateHPI_Failure()
        {
            // Act
            var srModel = new PresentIllnessViewModel
            {
                HPIID = -1,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.MinValue,
                ForceRollback = true
            };

            var response = controller.UpdateHPI(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Present Illness updated for invalid data.");
        }

        /// <summary>
        /// Deletes the hpi_ success.
        /// </summary>
        [TestMethod]
        public void DeleteHPI_Success()
        {
            // Act
            var response = controller.DeleteHPI(defaultDeleteID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Present Illness could not be deleted.");
        }

        /// <summary>
        /// Deletes the HPI_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteHPI_Failure()
        {
            // Act
            var response = controller.DeleteHPI(-1, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Present Illness deleted for invalid data.");
        }
    }
}
