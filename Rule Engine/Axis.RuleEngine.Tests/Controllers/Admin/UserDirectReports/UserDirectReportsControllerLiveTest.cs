using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Admin;
using Axis.Model.Common;

namespace Axis.RuleEngine.Tests.Controllers.Admin.UserDirectReports
{
    [TestClass]
    public class UserDirectReportsControllerLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "UserDirectReports/";

        /// <summary>
        /// The user identifier
        /// </summary>
        private int userID = 1;

        /// <summary>
        /// The mapping identifier
        /// </summary>
        private long mappingID = 11;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            _communicationManager = new CommunicationManager("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
            _communicationManager.UnitTestUrl = ConfigurationManager.AppSettings["UnitTestUrl"];
        }

        /// <summary>
        /// Gets the users_ success.
        /// </summary>
        [TestMethod]
        public void GetUsers_Success()
        {
            // Arrange
            var url = baseRoute + "GetUsers";
            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            // Act
            var response = _communicationManager.Get<Response<UserDirectReportsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one direct report must exist.");
        }

        /// <summary>
        /// Gets the users_ failure.
        /// </summary>
        [TestMethod]
        public void GetUsers_Failure()
        {
            // Arrange
            var url = baseRoute + "GetUsers";
            var param = new NameValueCollection();
            param.Add("userID", "-1");

            // Act
            var response = _communicationManager.Get<Response<UserDirectReportsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Direct report exists for invalid data.");
        }

        /// <summary>
        /// Gets the users by criteria_ success.
        /// </summary>
        [TestMethod]
        public void GetUsersByCriteria_Success()
        {
            // Arrange
            var url = baseRoute + "GetUsersByCriteria";
            var param = new NameValueCollection();
            param.Add("strSearch", "a");

            // Act
            var response = _communicationManager.Get<Response<UserDirectReportsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one User must exist with matching criteria.");
        }

        /// <summary>
        /// Gets the users by criteria_ failure.
        /// </summary>
        [TestMethod]
        public void GetUsersByCriteria_Failure()
        {
            // Arrange
            var url = baseRoute + "GetUsersByCriteria";
            var param = new NameValueCollection();
            param.Add("strSearch", "Invalid search string");

            // Act
            var response = _communicationManager.Get<Response<UserDirectReportsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "Direct report exists for invalid data.");
        }

        /// <summary>
        /// Add_s the success.
        /// </summary>
        [TestMethod]
        public void Add_Success()
        {
            // Arrange
            var url = baseRoute + "Add";
            var userReport = new UserDirectReportsModel
            {
                UserID = 21,
                ParentID = 12,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            // Act
            var response = _communicationManager.Post<UserDirectReportsModel, Response<UserDirectReportsModel>>(userReport, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "Direct Report could not be created.");
        }

        /// <summary>
        /// Add_s the failure.
        /// </summary>
        [TestMethod]
        public void Add_Failure()
        {
            // Arrange
            var url = baseRoute + "Add";
            var userReport = new UserDirectReportsModel
            {
                UserID = -1,
                ParentID = -12,
                ModifiedBy = 1,
                ForceRollback = true
            };

            // Act
            var response = _communicationManager.Post<UserDirectReportsModel, Response<UserDirectReportsModel>>(userReport, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "Direct Report created for invalid data.");
        }

        /// <summary>
        /// Remove_s the success.
        /// </summary>
        [TestMethod]
        public void Remove_Success()
        {
            // Arrange
            var url = baseRoute + "Remove";
            var param = new NameValueCollection();
            param.Add("id", mappingID.ToString());
            param.Add("modifiedOn", (DateTime.UtcNow).ToString());

            // Act
            var response = _communicationManager.Delete<Response<UserDirectReportsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected > 0, "Direct Report could not be deleted.");
        }

        /// <summary>
        /// Remove_s the failure.
        /// </summary>
        [TestMethod]
        public void Remove_Failure()
        {
            // Arrange
            var url = baseRoute + "Remove";
            var param = new NameValueCollection();
            param.Add("id", "-1");
            param.Add("modifiedOn", (DateTime.UtcNow).ToString());

            // Act
            var response = _communicationManager.Delete<Response<UserDirectReportsModel>>(param, url);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Direct Report is deleted for invalid data.");
        }

    }
}
