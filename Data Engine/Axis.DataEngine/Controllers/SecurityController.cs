using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Security;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityController : BaseApiController
    {
        /// <summary>
        /// The security data provider
        /// </summary>
        private ISecurityDataProvider securityDataProvider = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityController"/> class.
        /// </summary>
        /// <param name="securityDataProvider">The security data provider.</param>
        public SecurityController(ISecurityDataProvider securityDataProvider)
        {
            this.securityDataProvider = securityDataProvider;
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRoles(string roleName)
        {
            return new HttpResult<Response<RoleModel>>(securityDataProvider.GetRoles(roleName), Request);
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddRole(RoleModel role)
        {

            return new HttpResult<Response<RoleModel>>(securityDataProvider.AddRole(role), Request);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateRole(RoleModel role)
        {
            return new HttpResult<Response<RoleModel>>(securityDataProvider.UpdateRole(role), Request);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteRole(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<RoleModel>>(securityDataProvider.DeleteRole(id, modifiedOn), Request);
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetModule()
        {
            return new HttpResult<Response<ModuleModel>>(securityDataProvider.GetModule(), Request);
        }

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AssignModuleToRole(List<RoleModuleModel> role)
        {
            return new HttpResult<Response<RoleModuleModel>>(securityDataProvider.AssignModuleToRole(role), Request);
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetRoleById(long id)
        {
            return new HttpResult<Response<RoleModel>>(securityDataProvider.GetRoleById(id), Request);
        }

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetModuleByRoleId(long id)
        {
            return new HttpResult<Response<RoleModuleModel>>(securityDataProvider.GetModuleByRoleId(id), Request);
        }

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssignedPermissionByModuleId(long id, long roleId)
        {
            return new HttpResult<Response<PermissionModel>>(securityDataProvider.GetAssignedPermissionByModuleId(id, roleId), Request);
        }

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetAssignedPermissionByFeatureId(long id, long roleId)
        {
            return new HttpResult<Response<PermissionModel>>(securityDataProvider.GetAssignedPermissionByFeatureId(id, roleId), Request);
        }

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFeaturePermissionByModuleId(long id, long roleId)
        {
            return new HttpResult<Response<ModuleFeatureModel>>(securityDataProvider.GetFeaturePermissionByModuleId(id, roleId), Request);
        }

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetFeaturePermissionByRoleId(long roleId)
        {
            return new HttpResult<Response<ModuleFeatureModel>>(securityDataProvider.GetFeaturePermissionByRoleId(roleId), Request);
        }

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetModulePermissionByRoleId(long roleId)
        {
            return new HttpResult<Response<ModuleModel>>(securityDataProvider.GetModulePermissionByRoleId(roleId), Request);
        }

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AssignRolePermission(RolePermissionsModel role)
        {
            return new HttpResult<Response<RolePermissionsModel>>(securityDataProvider.AssignRolePermission(role), Request);
        }

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserRoleSecurity()
        {
            var userID = AuthContext.Auth.User.UserID;
            return new HttpResult<Response<RoleSecurityModel>>(securityDataProvider.GetUserRoleSecurity(userID), Request);
        }

        /// <summary>
        /// Verifies the role permission.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="moduleName">Name of the module.</param>
        /// <param name="featureName">Name of the feature.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult VerifyRolePermission(int userId, string modules, string permissionKey, string permission)
        {
            return new HttpResult<Response<ScalarResult<bool>>>(securityDataProvider.VerifyRolePermission(userId, modules, permissionKey, permission), Request);
        }

        [HttpGet]
        [SkipLogActionFilter]
        public IHttpActionResult GetUserCredentialSecurity()
        {
            var userID = AuthContext.Auth.User.UserID;
            return new HttpResult<Response<CredentialSecurityModel>>(securityDataProvider.GetUserCredentialSecurity(userID), Request);
        }

    }
}