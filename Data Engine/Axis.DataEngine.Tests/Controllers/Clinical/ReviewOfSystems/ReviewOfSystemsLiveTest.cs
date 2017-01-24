using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Common;
using Axis.Model.Clinical.ReviewOfSystems;

namespace Axis.DataEngine.Tests.Controllers.Clinical.ReviewOfSystems
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class ReviewOfSystemsLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "reviewOfSystems/";

        /// <summary>
        /// The ros identifier
        /// </summary>
        private long rosID = 7;

        /// <summary>
        /// The contact identifier
        /// </summary>
        private long contactID = 1;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Gets the review of systems by contact_ success.
        /// </summary>
        [TestMethod]
        public void GetReviewOfSystemsByContact_Success()
        {
            // Arrange
            var url = baseRoute + "GetReviewOfSystemsByContact";

            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "GetReviewOfSystemsByContact";

            var param = new NameValueCollection();
            param.Add("contactID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getReviewOfSystem";

            var param = new NameValueCollection();
            param.Add("rosID", rosID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getReviewOfSystem";

            var param = new NameValueCollection();
            param.Add("rosID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getLastActiveReviewOfSystems";

            var param = new NameValueCollection();
            param.Add("contactID", contactID.ToString());

            // Act
            var response = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getLastActiveReviewOfSystems";

            var param = new NameValueCollection();
            param.Add("contactID", "-1");

            // Act
            var response = communicationManager.Get<Response<ReviewOfSystemsModel>>(param, url);

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
            var url = baseRoute + "addReviewOfSystem";

            var ros = new ReviewOfSystemsModel
            {
                ContactID = 1,
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
            var response = communicationManager.Post<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(ros, url);

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
            var url = baseRoute + "addReviewOfSystem";

            var ros = new ReviewOfSystemsModel
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
            var response = communicationManager.Post<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(ros, url);

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
            var url = baseRoute + "updateReviewOfSystem";

            var ros = new ReviewOfSystemsModel
            {
                RoSID = 7,
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
            var response = communicationManager.Put<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(ros, url);

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
            var url = baseRoute + "updateReviewOfSystem";

            var ros = new ReviewOfSystemsModel
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
            var response = communicationManager.Put<ReviewOfSystemsModel, Response<ReviewOfSystemsModel>>(ros, url);

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
            // Arrange
            var url = baseRoute + "deleteReviewOfSystem";
            var param = new NameValueCollection();
            param.Add("rosID", rosID.ToString());

            // Act
            var response = communicationManager.Delete<Response<ReviewOfSystemsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected > 0, "Review Of System could not be deleted.");
        }

        /// <summary>
        /// Deletes the review of system_ failed.
        /// </summary>
        [TestMethod]
        public void DeleteReviewOfSystem_Failed()
        {
            // Arrange
            var url = baseRoute + "deleteReviewOfSystem";
            var param = new NameValueCollection();
            param.Add("rosID","-1");

            // Act
            var response = communicationManager.Delete<Response<ReviewOfSystemsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Delete review of system expected to be failed.");
        }
    }
}
