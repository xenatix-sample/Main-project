using System;
using Axis.DataEngine.Helpers.Results;
using Axis.DataEngine.Service.Controllers;
using Axis.DataProvider.Security;
using Axis.Model.Common;
using Axis.Model.Security;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Axis.DataEngine.Tests.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [TestClass]
    public class SecurityControllerTest
    {
        /// <summary>
        /// The security data provider
        /// </summary>
        private ISecurityDataProvider securityDataProvider;
        /// <summary>
        /// The role name
        /// </summary>
        private string roleName = string.Empty;
        /// <summary>
        /// The delete role identifier
        /// </summary>
        private long deleteRoleId = 1;
        /// <summary>
        /// The role identifier
        /// </summary>
        private long roleId = 2;
        /// <summary>
        /// The module role identifier
        /// </summary>
        private long moduleRoleId = 1;
        /// <summary>
        /// The permission module identifier
        /// </summary>
        private long permissionModuleId = 2;
        /// <summary>
        /// The permission feature identifier
        /// </summary>
        private long permissionFeatureId = 1;
        /// <summary>
        /// The module identifier
        /// </summary>
        private long ModuleId = 1;
        /// <summary>
        /// The feature module identifier
        /// </summary>
        private long FeatureModuleId = 1;
        /// <summary>
        /// The role modules
        /// </summary>
        private List<RoleModuleModel> roleModules = null;
        /// <summary>
        /// The role permission parameter
        /// </summary>
        private RolePermissionsModel rolePermissionParam = null;
        /// <summary>
        /// The security controller
        /// </summary>
        private SecurityController securityController = null;

        /// <summary>
        /// Initializes this instance.
        /// </summary>
        [TestInitialize]
        public void Initialize()
        {
            Mock<ISecurityDataProvider> mock = new Mock<ISecurityDataProvider>();
            securityDataProvider = mock.Object;

            securityController = new SecurityController(securityDataProvider);

            var roles = new List<RoleModel>();
            roles.Add(new RoleModel()
            {
                RoleID = 1,
                Name = "Administrator"
            });
            roles.Add(new RoleModel()
            {
                RoleID = 2,
                Name = "Front Desk-Clercial"
            });
            roles.Add(new RoleModel()
            {
                RoleID = 3,
                Name = "Billing-Back Office"
            });

            var allRoles = new Response<RoleModel>()
            {
                DataItems = roles
            };

            //Get Role
            Response<RoleModel> roleResponse = new Response<RoleModel>();
            roleResponse.DataItems = roles.Where(x => x.Name.Contains(roleName)).ToList();

            mock.Setup(r => r.GetRoles(It.IsAny<string>()))
                .Returns(roleResponse);

            //Add Role
            allRoles.RowAffected = 1;
            mock.Setup(r => r.AddRole(It.IsAny<RoleModel>()))
                .Callback((RoleModel roleModel) => roles.Add(roleModel))
                .Returns(allRoles);

            //Update Role
            mock.Setup(r => r.UpdateRole(It.IsAny<RoleModel>()))
                .Callback((RoleModel roleModel) => roles.Add(roleModel))
                .Returns(allRoles);

            //Delete Role
            Response<RoleModel> deleteResponse = new Response<RoleModel>();
            deleteResponse.RowAffected = 1;
            deleteResponse.DataItems = roles;

            mock.Setup(r => r.DeleteRole(It.IsAny<long>(), DateTime.UtcNow))
                .Callback((long id) => roles.Remove(roles.Find(deletedRole => deletedRole.RoleID == id)))
                .Returns(deleteResponse);

            //Get Module
            var modules = new List<ModuleModel>();
            modules.Add(new ModuleModel()
            {
                ModuleID = 1,
                Name = "Administrator"
            });
            modules.Add(new ModuleModel()
            {
                ModuleID = 2,
                Name = "Front Desk-Clercial"
            });
            modules.Add(new ModuleModel()
            {
                ModuleID = 3,
                Name = "Billing-Back Office"
            });

            var allModules = new Response<ModuleModel>()
            {
                DataItems = modules
            };

            mock.Setup(r => r.GetModule())
                .Returns(allModules);

            //Get Role By Id
            Response<RoleModel> roleResponseById = new Response<RoleModel>();
            roleResponseById.DataItems = roles.Where(x => x.RoleID == roleId).ToList();

            mock.Setup(r => r.GetRoleById(It.IsAny<long>()))
                .Returns(roleResponseById);

            //Get Module By roleId
            var moduleRole = new List<RoleModuleModel>();
            moduleRole.Add(new RoleModuleModel()
            {
                RoleId = 1,
                ModuleId = 1,
                RoleName = "Administrator",
                ModuleName = "Registration"
            });
            moduleRole.Add(new RoleModuleModel()
            {
                RoleId = 1,
                ModuleId = 2,
                RoleName = "Administrator",
                ModuleName = "Registration"
            });
            moduleRole.Add(new RoleModuleModel()
            {
                RoleId = 2,
                RoleName = "Administrator",
                ModuleName = "CPOE"
            });

            var allModuleRole = new Response<RoleModuleModel>()
            {
                DataItems = moduleRole
            };

            Response<RoleModuleModel> moduleResponseByRoleId = new Response<RoleModuleModel>();
            moduleResponseByRoleId.DataItems = moduleRole.Where(x => x.RoleId == moduleRoleId).ToList();

            mock.Setup(r => r.GetModuleByRoleId(It.IsAny<long>()))
                .Returns(moduleResponseByRoleId);

            Response<RoleModuleModel> assignRoleResponse = new Response<RoleModuleModel>();
            roleModules = moduleRole;
            assignRoleResponse.RowAffected = 1;
            assignRoleResponse.DataItems = moduleRole;

            //Assign Module To Role
            mock.Setup(r => r.AssignModuleToRole(It.IsAny<List<RoleModuleModel>>()))
                .Returns(assignRoleResponse);

            //Get Assigned Permission By moduleId
            var listModuleModel = new List<ModuleModel>();
            var lstModulePermissions = new List<PermissionModel>();
            lstModulePermissions.Add(new PermissionModel()
            {
                PermissionID = 1,
                PermissionName = "P1"
            });
            lstModulePermissions.Add(new PermissionModel()
            {
                PermissionID = 2,
                PermissionName = "P2"
            });

            listModuleModel.Add(new ModuleModel()
            {
                ModuleID = 1,
                Permissions = lstModulePermissions
            });
            listModuleModel.Add(new ModuleModel()
            {
                ModuleID = 2,
                Permissions = lstModulePermissions
            });

            Response<PermissionModel> permissionResponseByModuleId = new Response<PermissionModel>();
            permissionResponseByModuleId.DataItems = listModuleModel.Where(aa => aa.ModuleID == permissionModuleId).FirstOrDefault().Permissions;

            mock.Setup(r => r.GetAssignedPermissionByModuleId(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(permissionResponseByModuleId);

            //Get Assigned Permission By FeatureId
            var listFeatureModel = new List<FeatureModel>();
            var lstPermissions = new List<PermissionModel>();
            lstPermissions.Add(new PermissionModel()
            {
                PermissionID = 1,
                PermissionName = "P1"
            });
            lstPermissions.Add(new PermissionModel()
            {
                PermissionID = 2,
                PermissionName = "P2"
            });

            listFeatureModel.Add(new FeatureModel()
            {
                FeatureID = 1,
                Permissions = lstPermissions
            });
            listFeatureModel.Add(new FeatureModel()
            {
                FeatureID = 2,
                Permissions = lstPermissions
            });

            Response<PermissionModel> permissionResponseByFeatureId = new Response<PermissionModel>();
            permissionResponseByFeatureId.DataItems = listFeatureModel.Where(aa => aa.FeatureID == permissionFeatureId).FirstOrDefault().Permissions;

            mock.Setup(r => r.GetAssignedPermissionByFeatureId(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(permissionResponseByFeatureId);

            //Get Feature by moduleId
            var featureModule = new List<ModuleFeatureModel>();
            featureModule.Add(new ModuleFeatureModel()
            {
                ModuleId = 1,
                ModuleName = "Registration",
                FeatureName = "Feature1"
            });
            featureModule.Add(new ModuleFeatureModel()
            {
                ModuleId = 2,
                ModuleName = "CPOE",
                FeatureName = "Feature2"
            });

            var allFeatureModule = new Response<ModuleFeatureModel>()
            {
                DataItems = featureModule,
                RowAffected = 1
            };

            Response<ModuleFeatureModel> featureResponseByModuleId = new Response<ModuleFeatureModel>();
            featureResponseByModuleId.DataItems = featureModule.Where(x => x.ModuleId == FeatureModuleId).ToList();
            featureResponseByModuleId.RowAffected = 1;

            mock.Setup(r => r.GetFeaturePermissionByModuleId(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(featureResponseByModuleId);

            //Assign Role Permissions
            var rolePermissionList = new List<RolePermissionsModel>();

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

            rolePermissionList.Add(rolePermission);

            var moduleFeature = new List<FeatureModel>();
            var featurePermission = new List<PermissionModel>();
            featurePermission.Add(new PermissionModel()
            {
                PermissionID = 1
            });
            moduleFeature.Add(new FeatureModel()
            {
                FeatureID = 1,
                Permissions = featurePermission
            });
            rolePermission.Features = moduleFeature;

            rolePermissionList.Add(rolePermission);
            rolePermissionParam = rolePermission;

            var allRolePermission = new Response<RolePermissionsModel>()
            {
                DataItems = rolePermissionList
            };

            Response<RolePermissionsModel> rolePermissionResponse = new Response<RolePermissionsModel>();
            rolePermissionResponse.RowAffected = 1;
            rolePermissionResponse.DataItems = rolePermissionList;

            mock.Setup(r => r.AssignRolePermission(It.IsAny<RolePermissionsModel>()))
                .Returns(rolePermissionResponse);

            mock.Setup(r => r.GetFeaturePermissionByRoleId(It.IsAny<long>()))
                .Returns(featureResponseByModuleId);

              //Get Feature by moduleId
            var moduleModel = new List<ModuleModel>();
            moduleModel.Add(new ModuleModel()
            {
                ModuleID = 1,
                Name = "Module1",
                Permissions = lstModulePermissions
            });
            var allModule = new Response<ModuleModel>()
            {
                DataItems = moduleModel,
                RowAffected = 1
            };

            Response<ModuleModel> responseModule = new Response<ModuleModel>();
            responseModule.DataItems = moduleModel.Where(x => x.ModuleID == ModuleId).ToList();
            responseModule.RowAffected = 1;

            mock.Setup(r => r.GetModulePermissionByRoleId(It.IsAny<long>()))
                .Returns(responseModule);
        }

        /// <summary>
        /// Gets the roles_ success.
        /// </summary>
        [TestMethod]
        public void GetRoles_Success()
        {
            // Act
            var getRoleResult = securityController.GetRoles(roleName);
            var response = getRoleResult as HttpResult<Response<RoleModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(role.Count > 0, "Atleast one role must exists.");
        }

        /// <summary>
        /// Adds the role_ success.
        /// </summary>
        [TestMethod]
        public void AddRole_Success()
        {
            //Arrenge
            var addRole = new RoleModel
            {
                RoleID = 4,
                Name = "Physician"
            };

            // Act
            var actionResult = securityController.AddRole(addRole);
            var response = actionResult as HttpResult<Response<RoleModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(response.Value.RowAffected > 0, "Role could not be created.");
        }

        /// <summary>
        /// Updates the role_ success.
        /// </summary>
        [TestMethod]
        public void UpdateRole_Success()
        {
            //Arrenge
            var updateRole = new RoleModel
            {
                RoleID = 4,
                Name = "Phys"
            };

            // Act
            var actionResult = securityController.UpdateRole(updateRole);
            var response = actionResult as HttpResult<Response<RoleModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(response.Value.RowAffected > 0, "Role could not be updated.");
        }

        /// <summary>
        /// Deletes the role_ success.
        /// </summary>
        [TestMethod]
        public void DeleteRole_Success()
        {
            // Act
            var deleteRoleResult = securityController.DeleteRole(deleteRoleId, DateTime.UtcNow);
            var response = deleteRoleResult as HttpResult<Response<RoleModel>>;
            var deletedRole = response.Value;

            // Assert
            Assert.IsNotNull(deletedRole);
            Assert.IsTrue(deletedRole.RowAffected > 0, "Role could not be deleted.");
        }

        /// <summary>
        /// Gets the module_ success.
        /// </summary>
        [TestMethod]
        public void GetModule_Success()
        {
            // Act
            var getModuleResult = securityController.GetModule();
            var response = getModuleResult as HttpResult<Response<ModuleModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(role != null, "Data items can't be null");
            Assert.IsTrue(role.Count > 0, "Atleast one module must exists.");
        }

        /// <summary>
        /// Assigns the module to role_ success.
        /// </summary>
        [TestMethod]
        public void AssignModuleToRole_Success()
        {
            // Act
            var assignModuleToRoleResult = securityController.AssignModuleToRole(roleModules);
            var response = assignModuleToRoleResult as HttpResult<Response<RoleModuleModel>>;
            var role = response.Value;

            // Assert
            Assert.IsNotNull(role.DataItems);
            Assert.AreEqual(1, role.RowAffected);
            Assert.IsTrue(role.RowAffected > 0, "Module can not be assigned to Role");
        }

        /// <summary>
        /// Gets the role by id_ success.
        /// </summary>
        [TestMethod]
        public void GetRoleById_Success()
        {
            // Act
            var getRoleResultById = securityController.GetRoleById(roleId);
            var response = getRoleResultById as HttpResult<Response<RoleModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(role != null, "Data items can't be null");
            Assert.IsTrue(role.Count > 0, "Atleast one role must exists.");
        }

        /// <summary>
        /// Gets the module by role id_ success.
        /// </summary>
        [TestMethod]
        public void GetModuleByRoleId_Success()
        {
            // Act
            var getModuleResultByRoleId = securityController.GetModuleByRoleId(moduleRoleId);
            var response = getModuleResultByRoleId as HttpResult<Response<RoleModuleModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(role != null, "Data items can't be null");
            Assert.IsTrue(role.Count > 0, "Atleast one module for role must exists.");
        }

        /// <summary>
        /// Gets the assigned permission by module id_ success.
        /// </summary>
        [TestMethod]
        public void GetAssignedPermissionByModuleId_Success()
        {
            // Act
            var getPermissionResultByModuleId = securityController.GetAssignedPermissionByModuleId(permissionModuleId, roleId);
            var response = getPermissionResultByModuleId as HttpResult<Response<PermissionModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(role != null, "Data items can't be null");
            Assert.IsTrue(role.Count > 0, "Atleast one permission for module must exists.");
        }

        /// <summary>
        /// Gets the assigned permission by feature id_ success.
        /// </summary>
        [TestMethod]
        public void GetAssignedPermissionByFeatureId_Success()
        {
            // Act
            var getPermissionResultByFeatureId = securityController.GetAssignedPermissionByFeatureId(permissionFeatureId, roleId);
            var response = getPermissionResultByFeatureId as HttpResult<Response<PermissionModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(role != null, "Data items can't be null");
            Assert.IsTrue(role.Count > 0, "Atleast one permission for feature must exists.");
        }

        /// <summary>
        /// Gets the feature by module id_ success.
        /// </summary>
        [TestMethod]
        public void GetFeatureByModuleId_Success()
        {
            // Act
            var getFeatureResultByModuleId = securityController.GetFeaturePermissionByModuleId(FeatureModuleId, roleId);
            var response = getFeatureResultByModuleId as HttpResult<Response<ModuleFeatureModel>>;
            var role = response.Value.DataItems;

            // Assert
            Assert.IsNotNull(role);
            Assert.IsTrue(role != null, "Data items can't be null");
            Assert.IsTrue(role.Count > 0, "Atleast one feature for module must exists.");
        }

        /// <summary>
        /// Assigns the role permission_ success.
        /// </summary>
        [TestMethod]
        public void AssignRolePermission_Success()
        {
            // Act
            var assignRolePermissionResult = securityController.AssignRolePermission(rolePermissionParam);
            var response = assignRolePermissionResult as HttpResult<Response<RolePermissionsModel>>;
            var role = response.Value;

            // Assert
            Assert.IsNotNull(role.DataItems);
            Assert.AreEqual(1, role.RowAffected);
            Assert.IsTrue(role.RowAffected > 0, "Permission can not be assigned to Role");
        }

        /// <summary>
        /// Gets the feature permission by role id_ success.
        /// </summary>
        [TestMethod]
        public void GetFeaturePermissionByRoleId_Success()
        {
            // Act
            var featurePermissionByRoleResult = securityController.GetFeaturePermissionByRoleId(roleId);
            var response = featurePermissionByRoleResult as HttpResult<Response<ModuleFeatureModel>>;

            // Assert
            Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
        }

        /// <summary>
        /// Gets the module permission by role id_ success.
        /// </summary>
        [TestMethod]
        public void GetModulePermissionByRoleId_Success()
        {
            // Act
            var modulePermissionByRoleResult = securityController.GetModulePermissionByRoleId(roleId);
            var response = modulePermissionByRoleResult as HttpResult<Response<ModuleModel>>;

            // Assert
              Assert.IsNotNull(response);
            Assert.IsNotNull(response.Value);
            Assert.IsNotNull(response.Value.DataItems, "Data items can't be null");
        }
    }
}