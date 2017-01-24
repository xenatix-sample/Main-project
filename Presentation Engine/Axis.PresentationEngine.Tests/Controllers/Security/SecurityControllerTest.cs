using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Axis.PresentationEngine.Areas.Security.Repository;
using Axis.PresentationEngine.Areas.Security.Controllers;
using System.Linq;
using System.Web.Mvc;
using Axis.PresentationEngine.Areas.Security.Model;
using Newtonsoft.Json.Linq;
using Axis.PresentationEngine.Areas.Security.Translator;
using System.Web.Helpers;
using System.Collections.Generic;
using System.Xml.Linq;
using Axis.Model.Common;
using System.Configuration;

namespace Axis.PresentationEngine.Tests.Controllers
{

    [TestClass]
    public class SecurityControllerTest
    {
        private string roleName = string.Empty;
        private int roleId = 1;
        private int moduleRoleId = 1;
        private int permissionModuleId = 1;
        private int permissionFeatureId = 1;
        private int featureModuleId = 1;
        private RoleController controller = null;

        [TestInitialize]
        public void Initialize()
        {
            // Arrange
            controller = new RoleController(new SecurityRepository(ConfigurationManager.AppSettings["UnitTestToken"]));
        }

        [TestMethod]
        public void GetRoles_Success()
        {
            // Act
            var result = controller.GetRoles(roleName);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<RoleViewModel>;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void AddRole_Success()
        {
             //Act
            var roleViewModel = new RoleViewModel { RoleID = 1, Name = "Role3", Description = "Role3 Desc" };
            var result = controller.AddRole(roleViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<RoleViewModel>;
            var rowAffected = modelResponse.RowAffected;

             //Assert
            Assert.IsTrue(rowAffected > 0);
        }

        [TestMethod]
        public void UpdateRole_Success()
        {
             //Act
            var roleViewModel = new RoleViewModel { RoleID = 1, Name = "Role3", Description = "Role3 Desc Update" };
            var result = controller.UpdateRole(roleViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<RoleViewModel>;
            var rowAffected = modelResponse.RowAffected;

             //Assert
            Assert.IsTrue(rowAffected > 0);
        }

        [TestMethod]
        public void GetModule_Success()
        {
            // Act
            var result = controller.GetModule();
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<ModuleViewModel>;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void AssignModuleToRole_Success()
        {
            // Act
            var lstRoleModuleViewModel = new List<RoleModuleViewModel>();
            lstRoleModuleViewModel.Add(new RoleModuleViewModel { RoleId = 1, ModuleId = 1 });
            lstRoleModuleViewModel.Add(new RoleModuleViewModel { RoleId = 1, ModuleId = 2 });
            lstRoleModuleViewModel.Add(new RoleModuleViewModel { RoleId = 1, ModuleId = 3 });

            var result = controller.AssignModuleToRole(lstRoleModuleViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<RoleModuleViewModel>;
            var rowAffected = modelResponse.RowAffected;

            // Assert
            Assert.IsTrue(rowAffected > 0);
        }

        [TestMethod]
        public void GetRoleById_Success()
        {
            // Act
            var result = controller.GetRoleById(roleId);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<RoleViewModel>;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void GetModuleByRoleId_Success()
        {
            // Act
            var result = controller.GetModuleByRoleId(moduleRoleId);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<RoleModuleViewModel>;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void GetAssignedPermissionByModuleId_Success()
        {
            // Act
            var result = controller.GetAssignedPermissionByModuleId(permissionModuleId, roleId);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<PermissionViewModel>;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void GetAssignedPermissionByFeatureId_Success()
        {
            // Act
            var result = controller.GetAssignedPermissionByFeatureId(permissionFeatureId, roleId);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<PermissionViewModel>;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void GetFeatureByModuleId_Success()
        {
            // Act
            var result = controller.GetFeaturePermissionByModuleId(featureModuleId, roleId);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<ModuleFeatureViewModel>;
            var count = modelResponse.DataItems.Count;

            // Assert
            Assert.IsTrue(count > 0);
        }

        [TestMethod]
        public void AssignRolePermission_Success()
        {
            // Act
            var lstRolePermissionsViewModel = new List<RolePermissionsViewModel>();
            var rolePermissionsViewModel = new RolePermissionsViewModel();
            var role = new RoleViewModel();
            role.RoleID = 1;
            rolePermissionsViewModel.Role = role;
            var modules = new List<ModuleViewModel>();
            var permissions = new List<PermissionViewModel>();
            permissions.Add(new PermissionViewModel() { PermissionID = 1 });
            rolePermissionsViewModel.Modules = modules;
            modules.Add(new ModuleViewModel() { ModuleID = 1, Permissions = permissions });

            lstRolePermissionsViewModel.Add(new RolePermissionsViewModel { Role = role, Modules = modules });

            var features = new List<FeatureViewModel>();
            var permissionFeature = new List<PermissionViewModel>();
            permissionFeature.Add(new PermissionViewModel() { PermissionID = 1 });
            features.Add(new FeatureViewModel() { FeatureID = 1, Permissions = permissionFeature });
            rolePermissionsViewModel.Features = features;
            lstRolePermissionsViewModel.Add(new RolePermissionsViewModel { Role = role, Features = features });

            var result = controller.AssignRolePermission(rolePermissionsViewModel);
            var jsonResponse = result as JsonResult;
            var data = result.Data;
            var modelResponse = data as Response<RolePermissionsViewModel>;
            var rowAffected = modelResponse.RowAffected;

            // Assert
            Assert.IsTrue(rowAffected > 0);
        }
    }
}
