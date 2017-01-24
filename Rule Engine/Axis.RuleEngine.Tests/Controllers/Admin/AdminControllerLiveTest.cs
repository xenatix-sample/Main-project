using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Threading.Tasks;
using System.Web.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axis.RuleEngine.Tests.Controllers.Admin
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class AdminControllerLiveTest
    {
        #region Class Variables

        /// <summary>
        /// The HTTP client
        /// </summary>
        private HttpClient httpClient;

        #endregion

        #region Test Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            httpClient = new HttpClient();
            httpClient.BaseAddress = new Uri(ConfigurationManager.AppSettings["UnitTestUrl"]);
            httpClient.DefaultRequestHeaders.Add("X-Token", ConfigurationManager.AppSettings["UnitTestToken"]);
        }

        /// <summary>
        /// Tears down.
        /// </summary>
        [TestCleanup]
        public void TearDown()
        {
            httpClient.Dispose();
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetUsers()
        {
            var response = await httpClient.GetAsync("/api/Admin/GetUsers?firstName=&lastName=&userName=&isActive=true");
            var result = response.Content.ReadAsStringAsync().Result;
            var model = Json.Decode<Response<UserRoleModel>>(result);

            Assert.IsTrue(model.DataItems.Count > 0);
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task AddUser()
        {
            UserModel addModel = new UserModel()
            {
                UserID = 0,
                UserName = "TestUserUT",
                FirstName = "Test",
                LastName = "UserUT",
                Password = "test",
                EffectiveToDate = DateTime.Now,
                Roles = new List<RoleModel>() { },
                IsActive = false,
                ForceRollback = false
            };
            var response =
                await httpClient.PostAsync<UserModel>("/api/Admin/AddUser", addModel, new JsonMediaTypeFormatter());
            var result = response.Content.ReadAsStringAsync().Result;
            var responseModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(responseModel.ResultCode == 0, "Result Code must be 0 (Success)");
            Assert.IsTrue(responseModel.RowAffected == 1, "Rows Affected must equal 1");

            response =
                httpClient.GetAsync(
                    string.Format("/api/Admin/GetUsers?userName={0}&firstName={1}&lastName={2}&isActive={3}",
                        addModel.UserName, addModel.FirstName, addModel.LastName, addModel.IsActive.ToString())).Result;
            result = response.Content.ReadAsStringAsync().Result;
            var userModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(userModel.DataItems != null, "Data Items can't be null");
            Assert.IsTrue(userModel.DataItems.Count == 1, "Data Items must have exactly one item in it");
            Assert.AreEqual(addModel.UserName, userModel.DataItems.First().UserName, "Usernames must match");
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task UpdateUser()
        {
            UserModel updateModel = new UserModel()
            {
                UserID = 11,
                UserName = "tuser",
                FirstName = "Test",
                LastName = "User",
                Password = "Welcome1",
                EffectiveToDate = DateTime.Now,
                Roles = new List<RoleModel>() { },
                IsActive = false,
                ForceRollback = false
            };
            var response =
                await
                    httpClient.PostAsync<UserModel>("/api/Admin/UpdateUser", updateModel, new JsonMediaTypeFormatter());
            var result = response.Content.ReadAsStringAsync().Result;
            var responseModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(responseModel.ResultCode == 0, "Result Code must be 0 (Success)");
            Assert.IsTrue(responseModel.RowAffected == 1, "Rows Affected must equal 1");

            response =
                httpClient.GetAsync(
                    string.Format("/api/Admin/GetUsers?userName={0}&firstName={1}&lastName={2}&isActive={3}",
                        updateModel.UserName, updateModel.FirstName, updateModel.LastName, updateModel.IsActive.ToString())).Result;
            result = response.Content.ReadAsStringAsync().Result;
            var userModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(userModel.DataItems != null, "Data Items can't be null");
            Assert.IsTrue(userModel.DataItems.Count == 1, "Data Items must have exactly one item in it");
            Assert.AreEqual(updateModel.UserName, userModel.DataItems.First().UserName, "Usernames must match");
        }

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task RemoveUser()
        {
            UserModel removeModel = new UserModel()
            {
                UserID = 11,
                UserName = "tuser",
                FirstName = "Test",
                LastName = "User",
                Password = "Welcome1",
                EffectiveToDate = DateTime.Now,
                Roles = new List<RoleModel>() { },
                IsActive = false,
                ForceRollback = true
            };
            var response =
                await
                    httpClient.PostAsync<UserModel>("/api/Admin/RemoveUser", removeModel, new JsonMediaTypeFormatter());
            var result = response.Content.ReadAsStringAsync().Result;
            var responseModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(responseModel.ResultCode == 0, "Result Code must be 0 (Success)");
            Assert.IsTrue(responseModel.RowAffected == 1, "Rows Affected must equal 1");

            response =
               httpClient.GetAsync(
                   string.Format("/api/Admin/GetUsers?userName={0}&firstName={1}&lastName={2}&isActive={3}",
                       removeModel.UserName, removeModel.FirstName, removeModel.LastName, "false")).Result;
            result = response.Content.ReadAsStringAsync().Result;
            var userModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(userModel.DataItems.Count == 0, "Data Items must not have any items in it");
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ActivateUser()
        {
            UserModel activateModel = new UserModel()
            {
                UserID = 11,
                UserName = "tuser",
                FirstName = "Test",
                LastName = "User",
                Password = "Welcome1",
                EffectiveToDate = DateTime.Now,
                Roles = new List<RoleModel>() { },
                IsActive = true,
                ForceRollback = true
            };
            var response =
                await
                    httpClient.PostAsync<UserModel>("/api/Admin/ActivateUser", activateModel,
                        new JsonMediaTypeFormatter());
            var result = response.Content.ReadAsStringAsync().Result;
            var responseModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(responseModel.ResultCode == 0, "Result Code must be 0 (Success)");
            Assert.IsTrue(responseModel.RowAffected == 1, "Rows Affected must equal 1");
        }

        /// <summary>
        /// Resets the user.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task ResetUser()
        {
            UserModel resetModel = new UserModel()
            {
                UserID = 11,
                UserName = "tuser",
                FirstName = "Test",
                LastName = "User",
                Password = "Welcome1",
                EffectiveToDate = DateTime.Now,
                Roles = new List<RoleModel>() { },
                IsActive = true,
                ForceRollback = true
            };
            var response =
                await httpClient.PostAsync<UserModel>("/api/Admin/ResetUser", resetModel, new JsonMediaTypeFormatter());
            var result = response.Content.ReadAsStringAsync().Result;
            var responseModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(responseModel.ResultCode == 0, "Result Code must be 0 (Success)");
            Assert.IsTrue(responseModel.RowAffected == 1, "Rows Affected must equal 1");
        }

        /// <summary>
        /// Unlocks the user.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task UnlockUser()
        {
            UserModel resetModel = new UserModel()
            {
                UserID = 11,
                UserName = "tuser",
                FirstName = "Test",
                LastName = "User",
                Password = "Welcome1",
                EffectiveToDate = DateTime.Now,
                Roles = new List<RoleModel>() { },
                IsActive = true,
                ForceRollback = true
            };
            var response =
                await httpClient.PostAsync<UserModel>("/api/Admin/UnlockUser", resetModel, new JsonMediaTypeFormatter());
            var result = response.Content.ReadAsStringAsync().Result;
            var responseModel = Json.Decode<Response<UserModel>>(result);
            Assert.IsTrue(responseModel.ResultCode == 0, "Result Code must be 0 (Success)");
            Assert.IsTrue(responseModel.RowAffected == 1, "Rows Affected must equal 1");
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetUserRoles()
        {
            var response = await httpClient.GetAsync("/api/Admin/GetUserRoles?userID=3");
            var result = response.Content.ReadAsStringAsync().Result;
            var model = Json.Decode<Response<UserRoleModel>>(result);

            Assert.IsTrue(model.DataItems.Count > 0);
        }

        /// <summary>
        /// Gets the user credentials_ success.
        /// </summary>
        /// <returns></returns>
        [TestMethod]
        public async Task GetUserCredentials_Success()
        {
            //Arrange
            var response = await httpClient.GetAsync("/api/Admin/GetUserCredentials?userID=1");

            //Act
            var result = response.Content.ReadAsStringAsync().Result;
            var model = Json.Decode<Response<UserCredentialModel>>(result);

            //Assert
            Assert.IsNotNull(model);
            Assert.IsNotNull(model.DataItems);
            Assert.IsTrue(model.DataItems.Count > 0);

        }

        #endregion
    }
}
