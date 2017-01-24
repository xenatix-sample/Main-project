using System.Collections.Generic;
using System.Configuration;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.ApiControllers;
using Axis.PresentationEngine.Areas.Admin.Respository;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Axis.PresentationEngine.Tests.Controllers.Admin.UserRole
{
    [TestClass]
    public class UserRoleControllerLiveTest
    {
        private const string BaseRoute = "userRole/";
        private UserRoleController _controller;
        private readonly int _userID = 1;

        [TestInitialize]
        public void Initialize()
        {
            _controller = new UserRoleController(new UserRoleRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetUserRoles_Success()
        {
            var response = _controller.GetUserRoles(_userID);
            var hasAssignedRoles = false;
            foreach (var model in response.DataItems)
            {
                if (model.UserRoleID > 0)
                {
                    hasAssignedRoles = true;
                }
            }

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0 && hasAssignedRoles, "At least one user role record must exist");
        }

        [TestMethod]
        public void GetUserRoles_Failure()
        {
            var response = _controller.GetUserRoles(-1);
            var hasAssignedRoles = false;
            foreach(var model in response.DataItems)
            {
                if (model.UserRoleID > 0)
                {
                    hasAssignedRoles = true;
                }
            }

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(hasAssignedRoles == false, "Data exists for invalid user id");
        }

        [TestMethod]
        public void SaveUserRoles_Success()
        {
            var roles = new RoleModel(){ RoleID = 1, ForceRollback = true};
            var model = new UserViewModel()
            {
                UserID = _userID,
                Roles = new List<RoleModel>() { roles },
                ForceRollback = true
            };
            var response = _controller.SaveUserRoles(model);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.ResultCode == 0, "User roles could not be saved");
        }

        [TestMethod]
        public void SaveUserRoles_Failure()
        {
            var roles = new RoleModel() { RoleID = 1, ForceRollback = true};
            var model = new UserViewModel()
            {
                UserID = -1,
                Roles = new List<RoleModel>() { },
                ForceRollback = true
            };
            var response = _controller.SaveUserRoles(model);

            Assert.IsTrue(response != null, "Response can't be null");
            Assert.IsTrue(response.RowAffected == 0, "User roles were saved with invalid data.");
        }
    }
}
