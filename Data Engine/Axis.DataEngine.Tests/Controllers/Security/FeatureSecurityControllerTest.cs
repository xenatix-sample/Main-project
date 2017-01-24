using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.DataProvider.Security;
using Moq;
using System.Collections.Generic;
using Axis.Model.Security;
using Axis.Model.Common;
using Axis.DataEngine.Service.Controllers;
using Axis.DataEngine.Helpers.Results;

namespace Axis.DataEngine.Tests.Controllers.Security
{
    [TestClass]
    public class FeatureSecurityControllerTest
    {
        private ISecurityDataProvider securityDataProvider;
        private SecurityController securityController = null;
        private int userId = 1;
        private string modules = "Registration";
        private string permissionKey = "Registration";
        private string actionName = "Create";

        [TestInitialize]
        public void Initialize()
        {
            Mock<ISecurityDataProvider> mock = new Mock<ISecurityDataProvider>();
            securityDataProvider = mock.Object;

            securityController = new SecurityController(securityDataProvider);

            var roles = new List<RoleSecurityModel>();
            var permission = new List<UserPermissionModel>();
            permission.Add(new UserPermissionModel()
            {
                PermissionID = 1,
                PermissionName = "Create"
            });
            roles.Add(new RoleSecurityModel()
            {
                ModuleID = 1,
                ModuleName = "Registration",
                ComponentID = 1,
                ComponentName = "Demography",
                ModulePermissions = permission
            });

            var allRoleSeurity = new Response<RoleSecurityModel>()
            {
                DataItems = roles
            };

            //Get User Role Security
            mock.Setup(r => r.GetUserRoleSecurity(It.IsAny<int>())).Returns(allRoleSeurity);

            var rolePermissionResult = new List<ScalarResult<bool>>();
            rolePermissionResult.Add(new ScalarResult<bool>()
            {
                Result = true
            });

            mock.Setup(r => r.VerifyRolePermission(It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>())).Returns(new Response<ScalarResult<bool>>()
            {
                DataItems = rolePermissionResult
            });
        }

        [TestMethod]
        public void GetUserRoleSecurity_Success()
        {
            // Act
            var getUserRoleSecurityResult = securityController.GetUserRoleSecurity();

            // Arrange
            var response = getUserRoleSecurityResult as HttpResult<Response<RoleSecurityModel>>;
            var userRoleSecurity = response.Value.DataItems;

            //Assert
            Assert.IsNotNull(userRoleSecurity);
            Assert.IsTrue(userRoleSecurity.Count > 0, "Atleast one user role security must exists.");
        }

        [TestMethod]
        public void VerifyRolePermission_Success()
        {
            // Arrange
            var verifyRolePermissionResult = securityController.VerifyRolePermission(userId, modules, permissionKey, actionName);

            // Act
            var response = verifyRolePermissionResult as HttpResult<Response<ScalarResult<bool>>>;
            var userRoleSecurity = response.Value.DataItems;

            // Assert
            Assert.IsTrue(userRoleSecurity != null, "Data items can't be null");
            Assert.IsTrue(userRoleSecurity.Count > 0, "Atleast one role must exists.");
        }
    }
}
