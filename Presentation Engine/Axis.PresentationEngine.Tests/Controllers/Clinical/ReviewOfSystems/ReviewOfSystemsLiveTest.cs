using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Plugins.Clinical.ApiControllers;
using System.Configuration;
using Axis.Plugins.Clinical.Repository.ReviewOfSystems;
using System.Web.Mvc;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models.ReviewOfSystems;

namespace Axis.PresentationEngine.Tests.Controllers.Clinical.ReviewOfSystems
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ReviewOfSystemsLiveTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "reviewOfSystems/";

        /// <summary>
        /// The ros identifier
        /// </summary>
        private long rosID =1;

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactID = 1;

        /// <summary>
        /// The controller
        /// </summary>
        private ReviewOfSystemsController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new ReviewOfSystemsController(new ReviewOfSystemsRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the review of systems by contact_ success.
        /// </summary>
        [TestMethod]
        public void GetReviewOfSystemsByContact_Success()
        {
            // Act
            var response = controller.GetReviewOfSystemsByContact(contactID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one review of system must exist.");
        }

        /// <summary>
        /// Gets the review of systems by contact_ failed.
        /// </summary>
        [TestMethod]
        public void GetReviewOfSystemsByContact_Failed()
        {
            var response = controller.GetReviewOfSystemsByContact(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Review Of System should not exist for this test case.");
        }

        /// <summary>
        /// Gets the review of system_ success.
        /// </summary>
        [TestMethod]
        public void GetReviewOfSystem_Success()
        {
            // Act
            var response = controller.GetReviewOfSystem(1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one review of system must exist.");
        }

        /// <summary>
        /// Gets the review of system_ failed.
        /// </summary>
        [TestMethod]
        public void GetReviewOfSystem_Failed()
        {
            // Act
            var response = controller.GetReviewOfSystem(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Review Of System should not exist for this test case.");
        }

        /// <summary>
        /// Gets the last active review of systems_ success.
        /// </summary>
        [TestMethod]
        public void GetLastActiveReviewOfSystems_Success()
        {
            // Act
            var response = controller.GetLastActiveReviewOfSystems(contactID);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one review of system must exist.");
        }

        /// <summary>
        /// Gets the last active review of systems_ failed.
        /// </summary>
        [TestMethod]
        public void GetLastActiveReviewOfSystems_Failed()
        {
            // Act
            var response = controller.GetLastActiveReviewOfSystems(-1);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Review Of System should not exist for this test case.");
        }

        /// <summary>
        /// Adds the review of system_ success.
        /// </summary>
        [TestMethod]
        public void AddReviewOfSystem_Success()
        {
            // Arrange
            var ros = new ReviewOfSystemsViewModel
            {
                ContactID = 1,
                AssessmentID = 1,
                DateEntered = DateTime.UtcNow,
                IsReviewChanged =null ,
                LastAssessmentOn = null,
                ResponseID = 1,
                ReviewdBy=4,
                ReviewdByName="Chad Roberts",
                ForceRollback=true
            };

            // Act
            var response = controller.AddReviewOfSystem(ros);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Review Of System could not be created.");
        }

        /// <summary>
        /// Adds the review of system_ failed.
        /// </summary>
        [TestMethod]
        public void AddReviewOfSystem_Failed()
        {
            // Arrange
            var ros = new ReviewOfSystemsViewModel
            {
                ContactID = -1,
                AssessmentID = 1,
                DateEntered = DateTime.UtcNow,
                IsReviewChanged = null,
                LastAssessmentOn = null,
                ResponseID = 1,
                ReviewdBy = 4,
                ReviewdByName = "Chad Roberts",
                ForceRollback = true
            };

            // Act
            var response = controller.AddReviewOfSystem(ros);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Add review of system expected to be failed.");
        }

        /// <summary>
        /// Updates the review of system_ success.
        /// </summary>
        [TestMethod]
        public void UpdateReviewOfSystem_Success()
        {
            // Arrange
            var ros = new ReviewOfSystemsViewModel
            {
                RoSID=1,
                ContactID = 1,
                AssessmentID = 1,
                DateEntered = DateTime.UtcNow,
                IsReviewChanged = null,
                LastAssessmentOn = null,
                ResponseID = 1,
                ReviewdBy = 1,
                ReviewdByName = "Arun Choudhary",
                ForceRollback = true
            };

            // Act
            var response = controller.UpdateReviewOfSystem(ros);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Review Of System could not be updated.");
        }

        /// <summary>
        /// Updates the review of system_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateReviewOfSystem_Failed()
        {
            // Arrange
            var ros = new ReviewOfSystemsViewModel
            {
                RoSID = -1,
                ContactID = -1,
                AssessmentID = 1,
                DateEntered = DateTime.UtcNow,
                IsReviewChanged = null,
                LastAssessmentOn = null,
                ResponseID = 1,
                ReviewdBy = 1,
                ReviewdByName = "Arun Choudhary",
                ForceRollback = true
            };

            // Act
            var response = controller.UpdateReviewOfSystem(ros);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Update review of system expected to be failed.");
        }

        /// <summary>
        /// Deletes the review of system_ success.
        /// </summary>
        [TestMethod]
        public void DeleteReviewOfSystem_Success()
        {
            // Act
            var response = controller.DeleteReviewOfSystem(rosID, DateTime.UtcNow);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected > 0, "Review Of System could not be deleted.");
        }

        /// <summary>
        /// Deletes the review of system_ failure.
        /// </summary>
        [TestMethod]
        public void DeleteReviewOfSystem_Failed()
        {
            // Act
            var response = controller.DeleteReviewOfSystem(-1, DateTime.UtcNow);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Delete review of system expected to be failed.");
        }
    }
}
