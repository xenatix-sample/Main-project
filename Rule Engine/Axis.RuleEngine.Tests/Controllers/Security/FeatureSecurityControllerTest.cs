using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.RuleEngine.Security;
using Moq;
using Axis.Model.Security;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.RuleEngine.Service.Controllers;
using Axis.RuleEngine.Helpers.Results;

namespace Axis.RuleEngine.Tests.Controllers.Security
{
    [TestClass]
    public class FeatureSecurityControllerTest
    {
        private ISecurityRuleEngine securityRuleEngine;
        private SecurityController securityController = null;

        [TestInitialize]
        public void Initialize()
        {
            Mock<ISecurityRuleEngine> mock = new Mock<ISecurityRuleEngine>();
            securityRuleEngine = mock.Object;
            securityController = new SecurityController(securityRuleEngine);

            var roleSecurity = new List<RoleSecurityModel>();
            var permissions = new List<UserPermissionModel>();
            permissions.Add(new UserPermissionModel()
            {
                PermissionID = 1,
                PermissionName = "Create"
            });
            permissions.Add(new UserPermissionModel()
            {
                PermissionID = 2,
                PermissionName = "Delete"
            });

            roleSecurity.Add(new RoleSecurityModel()
            {
                ModuleID = 1,
                ModuleName = "Registration",
                ComponentID = 1,
                ComponentName = "Demography",
                ModulePermissions = permissions
            });

            var allRolesSeurity = new Response<RoleSecurityModel>()
            {
                DataItems = roleSecurity
            };

            //Get User Role Security
            mock.Setup(r => r.GetUserRoleSecurity()).Returns(allRolesSeurity);
        }

        [TestMethod]
        public void GetUserRoleSecurity_Success()
        {
            // Act
            var getUserRoleSecurityResult = securityController.GetUserRoleSecurity();
            var response = getUserRoleSecurityResult as HttpResult<Response<RoleSecurityModel>>;
            var userRoleSecurity = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(userRoleSecurity);
            Assert.IsTrue(userRoleSecurity.Count > 0, "Atleast one user role security must exists.");
        }
    }
}
