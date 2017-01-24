using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Model.Account;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Security;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Service.Controllers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Results;

namespace Axis.RuleEngine.Tests.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class AdminControllerTest
    {
        #region Class Variables

        /// <summary>
        /// The admin rule engine
        /// </summary>
        private IAdminRuleEngine adminRuleEngine;

        #endregion

        #region Test Methods

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Mock<IAdminRuleEngine> mock = new Mock<IAdminRuleEngine>();
            adminRuleEngine = mock.Object;

            var users = new List<UserModel>();
            users.Add(new UserModel()
            {
                UserID = 1,
                FirstName = "Test",
                LastName = "User",
                UserName = "tuser1",
                IsActive = true,
                EffectiveToDate = DateTime.Today.AddYears(2),
                LastLogin = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                LoginCount = 10,
                LoginAttempts = 0,
                ModifiedBy = 1,
                ModifiedOn = DateTime.Today,
                Password = "GYBH3gfg"
            });
            users.Add(new UserModel()
            {
                UserID = 2,
                FirstName = "Test",
                LastName = "User",
                UserName = "tuser2",
                IsActive = true,
                EffectiveToDate = DateTime.Today.AddYears(2),
                LastLogin = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                LoginCount = 10,
                LoginAttempts = 0,
                ModifiedBy = 1,
                ModifiedOn = DateTime.Today,
                Password = "GYBH3J88gfg"
            });
            users.Add(new UserModel()
            {
                UserID = 3,
                FirstName = "Test",
                LastName = "User",
                UserName = "tuser3",
                IsActive = true,
                EffectiveToDate = DateTime.Today.AddYears(2),
                LastLogin = DateTime.Now.Subtract(TimeSpan.FromDays(1)),
                LoginCount = 10,
                LoginAttempts = 0,
                ModifiedBy = 1,
                ModifiedOn = DateTime.Today,
                Password = "Vbnbg74h"
            });

            var allUsers = new Response<UserModel>()
            {
                DataItems = users
            };

            var userRoles = new List<UserRoleModel>();

            userRoles.Add(new UserRoleModel()
            {
                UserRoleID = 1,
                UserID = 1,
                RoleID = 1,
                Name = "System Admin",
                Description = "Full access across the application"
            });
            userRoles.Add(new UserRoleModel()
            {
                UserRoleID = 1,
                UserID = 2,
                RoleID = 2,
                Name = "Billing Admin",
                Description = "Only has access to the billing sections of the application"
            });
            userRoles.Add(new UserRoleModel()
            {
                UserRoleID = 1,
                UserID = 2,
                RoleID = 1,
                Name = "System Admin",
                Description = "Full access across the application"
            });

            var allUserRoles = new Response<UserRoleModel>()
            {
                DataItems = userRoles
            };

            //GetUsers
            Response<UserModel> userResponse = new Response<UserModel>();
            userResponse.DataItems = users.Where(u => u.UserName.Contains("")).ToList();
            mock.Setup(u => u.GetUsers(It.IsAny<string>())).Returns(allUsers);

            //Add User
            mock.Setup(u => u.AddUser(It.IsAny<UserModel>())).Callback((UserModel userModel) => users.Add(userModel)).Returns(allUsers);

            //UpdateUser
            mock.Setup(u => u.UpdateUser(It.IsAny<UserModel>())).Callback((UserModel userModel) => users.Add(userModel)).Returns(allUsers);

            //RemoveUser
            //Response<UserModel> removeUserResponse = new Response<UserModel>();
            //removeUserResponse.RowAffected = 1;
            //removeUserResponse.DataItems = users;

            //mock.Setup(u => u.RemoveUser(1, DateTime.UtcNow))).Callback((UserModel userModel) =>
            //    users.Remove(users.Find(deletedUser => deletedUser.UserID == userModel.UserID))).Returns(removeUserResponse);

            //ActivateUser
            mock.Setup(u => u.ActivateUser(It.IsAny<UserModel>())).Callback((UserModel userModel) => users.Add(userModel)).Returns(allUsers);

            //UnlockUser
            mock.Setup(u => u.UnlockUser(It.IsAny<UserModel>())).Callback((UserModel userModel) => users.Add(userModel)).Returns(allUsers);

            //GetUserRoles
            Response<UserRoleModel> userRoleResponse = new Response<UserRoleModel>();
            userRoleResponse.DataItems = userRoles.Where(u => u.Name.Contains("")).ToList();
            mock.Setup(u => u.GetUserRoles(It.IsAny<int>())).Returns(allUserRoles);

            //UserCredentialModel
            var userCredentialModel = new List<UserCredentialModel>();
            userCredentialModel.Add(new UserCredentialModel()
            {
                UserID = 1,
                CredentialName = "System Admin",
                CredentialID = 1,
                Description = "Full access across the application"
            });
            var allUserRoles1 = new Response<UserCredentialModel>()
            {
                DataItems = userCredentialModel,
                RowAffected = 1

            };

            //GetUsers
            Response<UserCredentialModel> userCredentialResponse = new Response<UserCredentialModel>();
            userCredentialResponse.DataItems = userCredentialModel.Where(u => u.CredentialName.Contains("System Admin")).ToList();
            userCredentialResponse.RowAffected = 1;
            mock.Setup(u => u.GetUserCredentials(It.IsAny<int>())).Returns(userCredentialResponse);
        }

        /// <summary>
        /// Gets the users.
        /// </summary>
        [TestMethod]
        public void GetUsers()
        {
            AdminController adminController = new AdminController(adminRuleEngine);
            var getUsersResult = adminController.GetUsers(string.Empty);
            var response = getUsersResult as HttpResult<Response<UserModel>>;
            var usersList = response.Value.DataItems;

            Assert.IsNotNull(usersList);
            Assert.IsTrue(usersList.Count > 0);
        }

        /// <summary>
        /// Adds the user.
        /// </summary>
        [TestMethod]
        public void AddUser()
        {
            AdminController adminController = new AdminController(adminRuleEngine);
            UserModel addModel = new UserModel() { UserID = 0, UserName = "TestUserUT", FirstName = "Test", LastName = "UserUT", EffectiveToDate = DateTime.Now, Roles = new List<RoleModel>() { }, IsActive = false, ForceRollback = true };
            var addUserResult = adminController.AddUser(addModel);
            var response = addUserResult as HttpResult<Response<UserModel>>;
            var data = response.Value.DataItems;

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Contains(addModel));
            Assert.AreEqual(4, data.Count());
        }

        /// <summary>
        /// Updates the user.
        /// </summary>
        [TestMethod]
        public void UpdateUser()
        {
            AdminController adminController = new AdminController(adminRuleEngine);
            UserModel updateModel = new UserModel() { UserID = 11, UserName = "tuser", FirstName = "Test", LastName = "User", EffectiveToDate = DateTime.Now, Roles = new List<RoleModel>() { }, IsActive = false, ForceRollback = true };
            var updateUserResult = adminController.UpdateUser(updateModel);
            var response = updateUserResult as HttpResult<Response<UserModel>>;
            var data = response.Value.DataItems;

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Contains(updateModel));
            Assert.AreEqual(4, data.Count());
        }

        /// <summary>
        /// Removes the user.
        /// </summary>
        [TestMethod]
        public void RemoveUser()
        {
            AdminController adminController = new AdminController(adminRuleEngine);
            var removeUserResult = adminController.RemoveUser(1, DateTime.UtcNow);
            var response = removeUserResult as HttpResult<Response<UserModel>>;
            var data = response.Value.DataItems;
            int rowsAffected = response.Value.RowAffected;

            Assert.IsNotNull(data);
            Assert.IsTrue(rowsAffected > 0);
            Assert.AreEqual(2, data.Count());
        }

        /// <summary>
        /// Activates the user.
        /// </summary>
        [TestMethod]
        public void ActivateUser()
        {
            AdminController adminController = new AdminController(adminRuleEngine);
            UserModel activateModel = new UserModel() { UserID = 11, UserName = "tuser", FirstName = "Test", LastName = "User", EffectiveToDate = DateTime.Now, Roles = new List<RoleModel>() { }, IsActive = false, ForceRollback = true };
            var activateUserResult = adminController.ActivateUser(activateModel);
            var response = activateUserResult as HttpResult<Response<UserModel>>;
            var data = response.Value.DataItems;
            int rowsAffected = response.Value.RowAffected;

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Contains(activateModel));
            Assert.AreEqual(4, data.Count());
        }

        /// <summary>
        /// Unlocks the user.
        /// </summary>
        [TestMethod]
        public void UnlockUser()
        {
            AdminController adminController = new AdminController(adminRuleEngine);
            UserModel unlockModel = new UserModel() { UserID = 11, UserName = "tuser", FirstName = "Test", LastName = "User", EffectiveToDate = DateTime.Now, Roles = new List<RoleModel>() { }, IsActive = false, ForceRollback = true };
            var unlockUserResult = adminController.UnlockUser(unlockModel);
            var response = unlockUserResult as HttpResult<Response<UserModel>>;
            int rowsAffected = response.Value.RowAffected;
            var data = response.Value.DataItems;

            Assert.IsNotNull(data);
            Assert.IsTrue(data.Contains(unlockModel));
            Assert.AreEqual(4, data.Count());
        }

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        [TestMethod]
        public void GetUserRoles()
        {
            AdminController adminController = new AdminController(adminRuleEngine);
            var getUserRolesResult = adminController.GetUserRoles(11);
            var response = getUserRolesResult as HttpResult<Response<UserRoleModel>>;
            var userRolesList = response.Value.DataItems;

            Assert.IsNotNull(userRolesList);
            Assert.IsTrue(userRolesList.Count > 0);
        }

        /// <summary>
        /// Gets the user credentials_ success.
        /// </summary>
        [TestMethod]
        public void GetUserCredentials_Success()
        {
            //Act
            AdminController adminController = new AdminController(adminRuleEngine);
            var getUserRolesResult = adminController.GetUserCredentials(11);
            var response = getUserRolesResult as HttpResult<Response<UserCredentialModel>>;

            //Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
            Assert.IsTrue(response.Value.RowAffected > 0);
        }

        #endregion
    }
}
