using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Configuration;
using Axis.PresentationEngine.Areas.Admin.ApiControllers;
using Axis.PresentationEngine.Areas.Admin.Respository.UserPhoto;
using Axis.PresentationEngine.Areas.Admin.Models;

namespace Axis.PresentationEngine.Tests.Controllers.Admin.UserPhoto
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class UserPhotoControllerLiveTest
    {
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "userPhoto/";

        /// <summary>
        /// The user identifier
        /// </summary>
        private int userID = 1;

        /// <summary>
        /// The user photo identifier
        /// </summary>
        private long userPhotoID = 1;

        private readonly bool isMyProfile = true;

        /// <summary>
        /// The controller
        /// </summary>
        private UserPhotoController controller = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            //Arrange
            controller = new UserPhotoController(new UserPhotoRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        /// <summary>
        /// Gets the user photo_ success.
        /// </summary>
        [TestMethod]
        public void GetUserPhoto_Success()
        {
            // Act
            var response = controller.GetUserPhoto(userID, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one user photo must exist.");
        }

        /// <summary>
        /// Gets the user photo_ failed.
        /// </summary>
        [TestMethod]
        public void GetUserPhoto_Failed()
        {
            var response = controller.GetUserPhoto(-1, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "User photo is exists for invalid data.");
        }

        /// <summary>
        /// Gets the user photo by id_ success.
        /// </summary>
        [TestMethod]
        public void GetUserPhotoById_Success()
        {
            // Act
            var response = controller.GetUserPhotoById(userPhotoID, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one user photo must exist.");
        }

        /// <summary>
        /// Gets the user photo by id_ failed.
        /// </summary>
        [TestMethod]
        public void GetUserPhotoById_Failed()
        {
            // Act
            var response = controller.GetUserPhotoById(-1, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems.Count == 0, "User photo is exists for invalid data.");
        }

        /// <summary>
        /// Adds the user photo_ success.
        /// </summary>
        [TestMethod]
        public void AddUserPhoto_Success()
        {
            // Arrange
            var userPhoto = new UserPhotoViewModel
            {
                UserID = 1,
                IsPrimary = true,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = controller.AddUserPhoto(userPhoto, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User photo could not be created.");
        }

        /// <summary>
        /// Adds the user photo_ failed.
        /// </summary>
        [TestMethod]
        public void AddUserPhoto_Failed()
        {
            // Arrange
            var userPhoto = new UserPhotoViewModel
            {
                UserID = -1,
                IsPrimary = true,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = controller.AddUserPhoto(userPhoto, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode != 0, "User photo is created for invalid data.");
        }

        /// <summary>
        /// Updates the user photo_ success.
        /// </summary>
        [TestMethod]
        public void UpdateUserPhoto_Success()
        {
            // Arrange
            var userPhoto = new UserPhotoViewModel
            {
                UserPhotoID = 1,
                UserID = 5,
                IsPrimary = false,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = controller.UpdateUserPhoto(userPhoto, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User photo could not be updated.");
        }

        /// <summary>
        /// Updates the user photo_ failed.
        /// </summary>
        [TestMethod]
        public void UpdateUserPhoto_Failed()
        {
            // Arrange
            var userPhoto = new UserPhotoViewModel
            {
                UserPhotoID = -1,
                UserID = -1,
                IsPrimary = false,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = controller.UpdateUserPhoto(userPhoto, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "User photo is updated for invalid data.");
        }

        /// <summary>
        /// Deletes the user photo_ success.
        /// </summary>
        [TestMethod]
        public void DeleteUserPhoto_Success()
        {
            // Act
            var response = controller.DeleteUserPhoto(userPhotoID, DateTime.UtcNow, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected > 0, "User photo could not be deleted.");
        }

        /// <summary>
        /// Deletes the user photo_ failed.
        /// </summary>
        [TestMethod]
        public void DeleteUserPhoto_Failed()
        {
            // Act
            var response = controller.DeleteUserPhoto(-1, DateTime.UtcNow, isMyProfile);

            // Assert
            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "User photo is deleted for invalid data.");
        }
    }
}
