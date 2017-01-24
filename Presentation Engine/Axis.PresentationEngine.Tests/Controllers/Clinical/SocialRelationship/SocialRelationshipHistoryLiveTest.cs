using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Clinical.ApiControllers;
using Axis.Plugins.Clinical;
using System.Configuration;
using Axis.Model.Common;

namespace Axis.PresentationEngine.Tests.Controllers.Clinical.SocialRelationship
{
    [TestClass]
    public class SocialRelationshipHistoryLiveTest
    {
        /// <summary>
        /// The default contact identifier
        /// </summary>
        //private int defaultContactId = 1;

        /// <summary>
        /// The default delete contact identifier
        /// </summary>
        private long defaultDeleteID = 10003;

        /// <summary>
        /// The controller
        /// </summary>
        private SocialRelationshipHistoryController controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new SocialRelationshipHistoryController(new SocialRelationshipHistoryRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        ///// <summary>
        ///// Gets the GetSocialRelationHistoryByContact success.
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationHistoryByContact_Success()
        //{
        //    // Act
        //    var result = controller.GetSocialRelationHistoryByContactAsync(defaultContactId).Result;
        //    var response = result.Data as Response<SocialRelationshipHistoryViewModel>;

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count > 0, "Atleast one Social Relationship must exist.");
        //}

        ///// <summary>
        ///// Gets the SocialRelationHistoryByContact failure.
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationHistoryByContact_Failure()
        //{
        //    // Act
        //    var result = controller.GetSocialRelationHistoryByContactAsync(-1).Result;
        //    var response = result.Data as Response<SocialRelationshipHistoryViewModel>;

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count == 0, "Social Relationship exists for invalid data.");
        //}

        /// <summary>
        /// Adds the SocialRelationship details success.
        /// </summary>
        [TestMethod]
        public void AddSocialRelationHistory_Success()
        {
            //Arrange
            var srModel = new SocialRelationshipHistoryViewModel
            {
                SocialRelationshipDetailID = 0,
                SocialRelationshipID = 10003,
                FamilyRelationshipID = 1,
                ChildhoodHistory = "No Children",
                RelationShipHistory = "No RelationShip",
                FamilyHistory = "Living with Family.",
                IsDeceased = false,
                IsInvolved = true,
                FirstName = "Peter",
                LastName = "Parker",
                RelationshipTypeID = 1,
                ReviewedNoChanges = false,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.AddSocialRelationHistory(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID > 0, "Social Relationship could not be created.");
        }

        /// <summary>
        /// Adds the SocialRelationship details failure.
        /// </summary>
        [TestMethod]
        public void AddSocialRelationHistory_Failure()
        {
            //Arrange
            var srModel = new SocialRelationshipHistoryViewModel
            {
                SocialRelationshipDetailID = 0,
                SocialRelationshipID = -1,
                FamilyRelationshipID = -1,
                ChildhoodHistory = "No Children",
                RelationShipHistory = "No RelationShip",
                FamilyHistory = "Living with Family.",
                IsDeceased = false,
                IsInvolved = true,
                FirstName = "Peter",
                LastName = "Parker",
                RelationshipTypeID = -1,
                ReviewedNoChanges = false,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.AddSocialRelationHistory(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.ID <= 0, "Social Relationship created for invalid data.");
        }

        /// <summary>
        /// Updates the SocialRelationship  details success.
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationHistory_Success()
        {
            //Arrange
            var srModel = new SocialRelationshipHistoryViewModel
            {
                SocialRelationshipDetailID = 1,
                SocialRelationshipID = 10003,
                FamilyRelationshipID = 1,
                ChildhoodHistory = "No Children",
                RelationShipHistory = "No RelationShip",
                FamilyHistory = "Living with Family.",
                IsDeceased = false,
                IsInvolved = true,
                FirstName = "Peter",
                LastName = "Parker",
                RelationshipTypeID = 1,
                ReviewedNoChanges = false,
                ContactID = 1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.UpdateSocialRelationHistory(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 0, "Social Relationship could not be updated.");
        }

        /// <summary>
        /// Updates the SocialRelationship details failure.
        /// </summary>
        [TestMethod]
        public void UpdateSocialRelationHistory_Failure()
        {
            //Arrange
            var srModel = new SocialRelationshipHistoryViewModel
            {
                SocialRelationshipDetailID = -1,
                SocialRelationshipID = 10003,
                FamilyRelationshipID = 1,
                ChildhoodHistory = "No Children",
                RelationShipHistory = "No RelationShip",
                FamilyHistory = "Living with Family.",
                IsDeceased = false,
                IsInvolved = true,
                FirstName = "Peter",
                LastName = "Parker",
                RelationshipTypeID = 1,
                ReviewedNoChanges = false,
                ContactID = -1,
                EncounterID = null,
                TakenBy = 1,
                TakenTime = DateTime.Now,
                ForceRollback = true
            };
            // Act
            var response = controller.UpdateSocialRelationHistory(srModel);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 0, "Social Relationship updated for invalid data.");
        }

        /// <summary>
        /// Deletes the SocialRelationship details success.
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationHistory_Success()
        {
            // Act
            var response = controller.DeleteSocialRelationHistory(defaultDeleteID, DateTime.UtcNow);

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected > 2, "Social Relationship could not be deleted.");
        }

        /// <summary>
        /// Deletes the SocialRelationship details failure.
        /// </summary>
        [TestMethod]
        public void DeleteSocialRelationHistory_Failure()
        {
            // Act
            var response = controller.DeleteSocialRelationHistory(9090, DateTime.UtcNow);      //some invalid ID

            // Assert
            Assert.IsNotNull(response, "Response can not be null");
            Assert.IsTrue(response.RowAffected == 2, "Social Relationship deleted for invalid data.");
        }

        ///// <summary>
        ///// GetSocialRelationshipDetail success test case
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationshipDetail_Success()
        //{
        //    //Arrange
        //    long socialRelationshipID = 10003;

        //    // Act
        //    var result = controller.GetSocialRelationshipDetail(socialRelationshipID);
        //    var response = result.Data as Response<SocialRelationshipHistoryViewModel>;

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count > 0, "Atleast one record should exist.");
        //}

        ///// <summary>
        ///// GetSocialRelationshipDetail success test case
        ///// </summary>
        //[TestMethod]
        //public void GetSocialRelationshipDetail_Failure()
        //{
        //    //Arrange
        //    long socialRelationshipID = -1;

        //    // Act
        //    var result = controller.GetSocialRelationshipDetail(socialRelationshipID);
        //    var response = result.Data as Response<SocialRelationshipHistoryViewModel>;

        //    // Assert
        //    Assert.IsNotNull(response, "Response can not be null");
        //    Assert.IsNotNull(response.DataItems, "DataItems can not be null");
        //    Assert.IsTrue(response.DataItems.Count == 0, "Details exist for invalid record.");
        //}
    }
}
