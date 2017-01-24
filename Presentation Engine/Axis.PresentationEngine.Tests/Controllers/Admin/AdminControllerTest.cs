using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Web.Mvc;
using Axis.PresentationEngine.Areas.Admin.Controllers;


namespace Axis.PresentationEngine.Tests._controllers
{
    [TestClass]
    public class Admin_controllerTest
    {
        #region Class Variables

        private AdminController _controller;

        #endregion

        #region Test Methods

        [TestInitialize]
        public void Initialize()
        {
            _controller = new AdminController(new AdminRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        #region ActionResults

        [TestMethod]
        public void Index_Success()
        {
            ActionResult result = _controller.Index();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void IndexInstance_Success()
        {
            ActionResult result = _controller.Index();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Add_Success()
        {
            ActionResult result = _controller.Add();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void AddInstance_Success()
        {
            ActionResult result = _controller.Add();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        [TestMethod]
        public void Edit_Success()
        {
            ActionResult result = _controller.Edit();

            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void EditInstance_Success()
        {
            ActionResult result = _controller.Edit();

            Assert.IsInstanceOfType(result, typeof(ViewResult));
        }

        #endregion

        #region JsonResults

        [TestMethod]
        public void GetUsers_Success()
        {
            var result = _controller.GetUsers(string.Empty);
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserModel>)data;
            var count = modelResponse.DataItems.Count;

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void GetUsersSearch_Success()
        {
            var result = _controller.GetUsers("tuser");
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserModel>)data;
            var count = modelResponse.DataItems.Count;

            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void AddUser_Success()
        {
            var result = _controller.AddUser(new UserViewModel() { UserID = 0, UserName = "TestUserUT", FirstName = "Test", LastName = "UserUT", EffectiveToDate = DateTime.Now, Roles = new List<RoleModel>() { }, IsActive = false, ForceRollback = true });
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserModel>)data;
            var rowsAffected = modelResponse.RowAffected;

            Assert.IsTrue(rowsAffected > 0);
        }

        [TestMethod]
        public void UpdateUser_Success()
        {
            var result = _controller.UpdateUser(new UserViewModel() { UserID = 11, UserName = "tuser", FirstName = "Test", LastName = "User", EffectiveToDate = DateTime.Now, Roles = new List<RoleModel>() { }, IsActive = false, ForceRollback = true });
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserModel>)data;
            var rowsAffected = modelResponse.RowAffected;

            Assert.IsTrue(rowsAffected > 0);
        }

        [TestMethod]
        public void RemoveUser_Success()
        {
            var result = _controller.RemoveUser(new UserViewModel() { UserID = 11, ForceRollback = true });
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserModel>)data;
            var rowsAffected = modelResponse.RowAffected;

            Assert.IsTrue(rowsAffected > 0);
        }

        [TestMethod]
        public void ActivateUser_Success()
        {
            var result = _controller.ActivateUser(new UserViewModel() { UserID = 11, ForceRollback = true });
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserModel>)data;
            var rowsAffected = modelResponse.RowAffected;

            Assert.IsTrue(rowsAffected > 0);
        }

        [TestMethod]
        public void UnlockUser_Success()
        {
            var result = _controller.UnlockUser(new UserViewModel() { UserID = 11, ForceRollback = true });
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserModel>)data;
            var rowsAffected = modelResponse.RowAffected;

            Assert.IsTrue(rowsAffected > 0);
        }

        [TestMethod]
        public void GetUserRoles_Success()
        {
            var result = _controller.GetUserRoles(11);
            var jsonResponse = (JsonResult)result;
            var data = result.Data;
            var modelResponse = (Response<UserRoleModel>)data;
            var count = modelResponse.DataItems.Count;

            //Update the deployment script to include a user with roles assigned
            //Assert.IsTrue(count > 0);
            Assert.IsTrue(modelResponse.ResultCode == 0);
        }

        #endregion

        #endregion
    }
}
