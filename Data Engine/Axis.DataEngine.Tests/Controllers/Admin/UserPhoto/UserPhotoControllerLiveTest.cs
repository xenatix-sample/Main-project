using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.Service;
using System.Configuration;
using System.Collections.Specialized;
using Axis.Model.Admin;
using Axis.Model.Common;

namespace Axis.DataEngine.Tests.Controllers.Admin.UserPhoto
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class UserPhotoControllerLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;

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
        private long userPhotoID = 2;

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
        /// Gets the user photo_ success.
        /// </summary>
        [TestMethod]
        public void GetUserPhoto_Success()
        {
            // Arrange
            var url = baseRoute + "getUserPhoto";

            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());

            // Act
            var response = communicationManager.Get<Response<UserPhotoModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getUserPhoto";

            var param = new NameValueCollection();
            param.Add("userID", "-1");

            // Act
            var response = communicationManager.Get<Response<UserPhotoModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getUserPhotoById";

            var param = new NameValueCollection();
            param.Add("userPhotoID", userPhotoID.ToString());

            // Act
            var response = communicationManager.Get<Response<UserPhotoModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "getUserPhotoById";

            var param = new NameValueCollection();
            param.Add("userPhotoID", "-1");

            // Act
            var response = communicationManager.Get<Response<UserPhotoModel>>(param, url);

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
            var url = baseRoute + "addUserPhoto";

            var userPhoto = new UserPhotoModel
            {
                UserID = 1,
                IsPrimary = true,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<UserPhotoModel, Response<UserPhotoModel>>(userPhoto, url);

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
            var url = baseRoute + "addUserPhoto";

            var userPhoto = new UserPhotoModel
            {
                UserID = -1,
                IsPrimary = true,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Post<UserPhotoModel, Response<UserPhotoModel>>(userPhoto, url);

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
            var url = baseRoute + "updateUserPhoto";

            var userPhoto = new UserPhotoModel
            {
                UserPhotoID = 1,
                UserID = 5,
                IsPrimary = false,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<UserPhotoModel, Response<UserPhotoModel>>(userPhoto, url);

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
            var url = baseRoute + "updateUserPhoto";

            var userPhoto = new UserPhotoModel
            {
                UserPhotoID = -1,
                UserID = -1,
                IsPrimary = false,
                PhotoID = 1,
                ForceRollback = true
            };

            // Act
            var response = communicationManager.Put<UserPhotoModel, Response<UserPhotoModel>>(userPhoto, url);

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
            // Arrange
            var url = baseRoute + "deleteUserPhoto";
            var param = new NameValueCollection();
            param.Add("userPhotoID", userPhotoID.ToString());
            param.Add("modifiedOn",(DateTime.UtcNow).ToString());

            // Act
            var response = communicationManager.Delete<Response<UserPhotoModel>>(param, url);

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
            // Arrange
            var url = baseRoute + "deleteUserPhoto";
            var param = new NameValueCollection();
            param.Add("userPhotoID", "-1");
            param.Add("modifiedOn", (DateTime.UtcNow).ToString());

            // Act
            var response = communicationManager.Delete<Response<UserPhotoModel>>(param, url);

            // Assert

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "User photo is deleted for invalid data.");
        }
    }
}
