using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.PresentationEngine.Areas.Admin.ApiControllers;
using Axis.PresentationEngine.Areas.Admin.Respository;
using System.Configuration;
using Axis.PresentationEngine.Areas.Admin.Models;

namespace Axis.PresentationEngine.Tests.Controllers.Admin.UserDirectReport
{
    [TestClass]
    public class UserDirectReportControllerLiveTest
    {
        /// <summary>
        /// The user identifier
        /// </summary>
        private int userID = 1;

        /// <summary>
        /// The mapping identifier
        /// </summary>
        private long mappingID = 11;

        private readonly bool isMyProfile = true;

        /// <summary>
        /// The controller
        /// </summary>
        private UserDirectReportsController _controller;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            _controller = new UserDirectReportsController(new UserDirectReportsRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the users_ success.
        /// </summary>
        [TestMethod]
        public void GetUsers_Success()
        {
            // Act
            var response = _controller.GetUsers(userID, isMyProfile);

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
            // Act
            var response = _controller.GetUsers(-1, isMyProfile);

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
            // Act
            var response = _controller.GetUsersByCriteria("a", isMyProfile);

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
            // Act
            var response = _controller.GetUsersByCriteria("Invalid matching string", isMyProfile);     //passing text for which user doesnot exist

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
            var userReport = new UserDirectReportsViewModel
            {
                UserID = 21,
                ParentID = 12,
                ModifiedOn = DateTime.Now,
                ForceRollback = true
            };

            // Act
            var response = _controller.Add(userReport, isMyProfile);

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
            var userReport = new UserDirectReportsViewModel
            {
                UserID = -1,
                ParentID = -12,
                ModifiedBy = 1,
                ForceRollback = true
            };

            // Act
            var response = _controller.Add(userReport, isMyProfile);

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
            // Act
            var response = _controller.Remove(mappingID, DateTime.UtcNow, isMyProfile);

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
            // Act
            var response = _controller.Remove(-1, DateTime.UtcNow, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "Direct Report is deleted for invalid data.");
        }

    }
}
