using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Axis.DataProvider.Security;
using Axis.DataEngine.Service.Controllers;
using Axis.Model.Security;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Web.Http;
using System.Linq;
using Axis.DataEngine.Helpers.Results;
using System.Net.Http;
using System.Web.Http.Results;
using Axis.Service;
using System.Web.Mvc;
using System.Collections.Specialized;
using System.Configuration;

namespace Axis.DataEngine.Tests.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SecurityControllerLiveTest
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private CommunicationManager communicationManager;
        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "security/";
        /// <summary>
        /// The role name
        /// </summary>
        private string roleName = "";
        /// <summary>
        /// The delete role identifier
        /// </summary>
        private long deleteRoleID = 1;
        /// <summary>
        /// The role identifier
        /// </summary>
        private long roleId = 1;
        /// <summary>
        /// The module identifier
        /// </summary>
        private long moduleId = 1;
        /// <summary>
        /// The feature identifier
        /// </summary>
        private long featureId = 1;

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
        /// Gets the roles_ success.
        /// </summary>
        [TestMethod]
        public void GetRoles_Success()
        {
            var url = baseRoute + "getRoles";

            var param = new NameValueCollection();
            param.Add("roleName", roleName ?? string.Empty);

            var response = communicationManager.Get<Response<RoleModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one role must exists.");
        }

        /// <summary>
        /// Adds the role_ success.
        /// </summary>
        [TestMethod]
        public void AddRole_Success()
        {
            var url = baseRoute + "addRole";

            var addRole = new RoleModel
            {
                Name = "Physician",
                Description = "Unit test for role"
            };

            var response = communicationManager.Post<RoleModel, Response<RoleModel>>(addRole, url);

            Assert.IsTrue(response.RowAffected > 0, "Role could not be created.");
        }

        /// <summary>
        /// Updateds the role_ success.
        /// </summary>
        [TestMethod]
        public void UpdatedRole_Success()
        {
            var url = baseRoute + "updateRole";

            var updateRole = new RoleModel
            {
                RoleID = 2,
                Name = "UnitTest",
                Description = "Unit test for role"
            };

            var response = communicationManager.Post<RoleModel, Response<RoleModel>>(updateRole, url);

            Assert.IsTrue(response.RowAffected > 0, "Role could not be updated.");
        }

        /// <summary>
        /// Deletes the role_ success.
        /// </summary>
        [TestMethod]
        public void DeleteRole_Success()
        {
            var url = baseRoute + "deleteRole";
            communicationManager.Delete(deleteRoleID, url);
        }

        /// <summary>
        /// Gets the module_ success.
        /// </summary>
        [TestMethod]
        public void GetModule_Success()
        {
            var url = baseRoute + "getModule";

            var response = communicationManager.Get<Response<ModuleModel>>(url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one module must exists.");
        }

        /// <summary>
        /// Assigns the module to role_ success.
        /// </summary>
        [TestMethod]
        public void AssignModuleToRole_Success()
        {
            var url = baseRoute + "assignModuleToRole";

            //Assign Module To Role
            var moduleRole = new List<RoleModuleModel>();

            moduleRole.Add(new RoleModuleModel()
            {
                RoleId = 6,
                ModuleId = 1,
                RoleName = "Administrator",
                ModuleName = "Registration"
            });
            moduleRole.Add(new RoleModuleModel()
            {
                RoleId = 6,
                ModuleId = 2,
                RoleName = "Administrator",
                ModuleName = "Registration"
            });
            moduleRole.Add(new RoleModuleModel()
            {
                RoleId = 8,
                ModuleId = 1,
                RoleName = "Administrator",
                ModuleName = "CPOE"
            });

            var response = communicationManager.Post<List<RoleModuleModel>, Response<RoleModuleModel>>(moduleRole, url);

            Assert.IsTrue(response.RowAffected > 0, "Module can not be assigned to Role");
        }

        /// <summary>
        /// Gets the role by id_ success.
        /// </summary>
        [TestMethod]
        public void GetRoleById_Success()
        {
            var url = baseRoute + "getRoleById";

            var param = new NameValueCollection();
            param.Add("id", roleId.ToString());

            var response = communicationManager.Get<Response<RoleModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one role must exists.");
        }

        /// <summary>
        /// Gets the module by role id_ success.
        /// </summary>
        [TestMethod]
        public void GetModuleByRoleId_Success()
        {
            var url = baseRoute + "getModuleByRoleId";

            var param = new NameValueCollection();
            param.Add("id", roleId.ToString());

            var response = communicationManager.Get<Response<RoleModuleModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one module for role must exists.");
        }

        /// <summary>
        /// Gets the assigned permission by module id_ success.
        /// </summary>
        [TestMethod]
        public void GetAssignedPermissionByModuleId_Success()
        {
            var url = baseRoute + "getAssignedPermissionByModuleId";

            var param = new NameValueCollection();
            param.Add("id", moduleId.ToString());
            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<PermissionModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one permission for module must exists.");
        }

        /// <summary>
        /// Gets the assigned permission by feature id_ success.
        /// </summary>
        [TestMethod]
        public void GetAssignedPermissionByFeatureId_Success()
        {
            var url = baseRoute + "getAssignedPermissionByFeatureId";

            var param = new NameValueCollection();
            param.Add("id", featureId.ToString());
            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<PermissionModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one permission for feature must exists.");
        }

        /// <summary>
        /// Gets the feature by module id_ success.
        /// </summary>
        [TestMethod]
        public void GetFeatureByModuleId_Success()
        {
            var url = baseRoute + "getFeaturePermissionByModuleId";

            var param = new NameValueCollection();
            param.Add("id", moduleId.ToString());
            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<ModuleFeatureModel>>(param, url);

            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one feature for module must exists.");
        }

        /// <summary>
        /// Assigns the role permission_ success.
        /// </summary>
        [TestMethod]
        public void AssignRolePermission_Success()
        {
            var url = baseRoute + "assignRolePermission";

            //Assign Role Permissions
            var rolePermission = new RolePermissionsModel();
            rolePermission.Role = new RoleModel()
            {
                RoleID = 1
            };
            var modulePermission = new List<ModuleModel>();
            modulePermission.Add(new ModuleModel()
            {
                ModuleID = 1
            });
            rolePermission.Modules = modulePermission;

            var moduleFeature = new List<FeatureModel>();
            var featurePermission = new List<PermissionModel>();
            featurePermission.Add(new PermissionModel()
            {
                PermissionID = 1,
                PermissionName = "Create"
            });
            moduleFeature.Add(new FeatureModel()
            {
                FeatureID = 1,
                Permissions = featurePermission
            });
            rolePermission.Features = moduleFeature;

            var response = communicationManager.Post<RolePermissionsModel, Response<RolePermissionsModel>>(rolePermission, url);

            Assert.IsTrue(response.RowAffected > 0, "Permission can not be assigned to Role");
        }

        /// <summary>
        /// Gets the feature permission by role id_ success.
        /// </summary>
        [TestMethod]
        public void GetFeaturePermissionByRoleId_Success()
        {
            //Arrenge
            var url = baseRoute + "getFeaturePermissionByRoleId";
            var param = new NameValueCollection();
            param.Add("id", roleId.ToString());

            //Act
            var response = communicationManager.Get<Response<ModuleFeatureModel>>(param, url);

            //Assert
            Assert.IsTrue(response.DataItems != null, "Data items can't be null");
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one role must exists.");
        }

        /// <summary>
        /// Gets the module permission by role id_ success.
        /// </summary>
        [TestMethod]
        public void GetModulePermissionByRoleId_Success()
        {
            //Arrenge
            var url = baseRoute + "getModulePermissionByRoleId";
            var param = new NameValueCollection();
            param.Add("id", roleId.ToString());

            // Act
            var response = communicationManager.Get<Response<ModuleModel>>(param, url);

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.DataItems);
            Assert.IsTrue(response.DataItems.Count > 0, "Atleast one role must exists.");



        }
    }
}