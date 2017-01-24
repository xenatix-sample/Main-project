using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Clinical.Repository.SocialRelationship;
using Axis.Plugins.Clinical.ApiControllers;
using System.Configuration;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.SocialRelationship;

namespace Axis.PresentationEngine.Tests.Controllers.Clinical.SocialRelationship
{
    [TestClass]
    public class SocialRelationshipLiveTest
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
        private SocialRelationshipController controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new SocialRelationshipController(new SocialRelationshipRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the GetSocialRelationshipsByContact_success.
        /// </summary>
        [TestMethod]
        public void GetSocialRelationshipsByContact_Success()
        {
            // Act
            var response = controller.GetSocialRelationshipsByContact(defaultContactId);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Social Relationship must exist.");
        }

        /// <summary>
        /// Gets the GetSocialRelationshipsByContact_failure.
        /// </summary>
        [TestMethod]
        public void GetSocialRelationshipsByContact_Failure()
        {
            // Act
            var response = controller.GetSocialRelationshipsByContact(-1);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsNotNull(response.DataItems, "DataItems can not be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Social Relationship exists for invalid data.");
        }

        /// <summary>
        /// Adds the SocialRelationship_ success.
        /// </summary>
        [TestMethod]
        public void AddSocialRelationship_Success()
        {
            // Act
            var srModel = new SocialRelationshipViewModel
            {
                SocialRelationshipID = 0,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddSocialRelationship(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID > 0, "Social Relationship could not be created.");
        }

        /// <summary>
        /// Adds the SocialRelationship_ failure.
        /// </summary>
        [TestMethod]
        public void AddSocialRelationship_Failure()
        {
            // Act
            var srModel = new SocialRelationshipViewModel
            {
                SocialRelationshipID = 0,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.AddSocialRelationship(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID <= 0, "Social Relationship created for invalid data.");
        }

        /// <summary>
        /// Updates the SocialRelationship_ success.
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationship_Success()
        {
            // Act
            var srModel = new SocialRelationshipViewModel
            {
                SocialRelationshipID = 1,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };

            var response = controller.UpdateSocialRelationship(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Social Relationship could not be updated.");
        }

        /// <summary>
        /// Updates the SocialRelationship_ failure.
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationship_Failure()
        {
            // Act
            var srModel = new SocialRelationshipViewModel
            {
                SocialRelationshipID = -1,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.MinValue,
                ForceRollback = true
            };

            var response = controller.UpdateSocialRelationship(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Social Relationship updated for invalid data.");
        }

        /// <summary>
        /// Deletes the SocialRelationship_ success.
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationship_Success()
        {
            // Act
            var response = controller.DeleteSocialRelationship(defaultDeleteID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Social Relationship could not be deleted.");
        }

        /// <summary>
        /// Deletes the SocialRelationship_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationship_Failure()
        {
            // Act
            var response = controller.DeleteSocialRelationship(-1, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Social Relationship deleted for invalid data.");
        }
    }
}
