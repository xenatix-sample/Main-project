using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Service.Security;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Security
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityRuleEngine : ISecurityRuleEngine
    {
        /// <summary>
        /// The security service
        /// </summary>
        private ISecurityService securityService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityRuleEngine"/> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        public SecurityRuleEngine(ISecurityService securityService)
        {
            this.securityService = securityService;
        }

        #region Role

        /// <summary>
        /// Gets the roles.
        /// TO DO - Paging will be implemented using Request object
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public Response<RoleModel> GetRoles(string roleName)
        {
            return securityService.GetRoles(roleName);
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModel> AddRole(RoleModel role)
        {
            return securityService.AddRole(role);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModel> UpdateRole(RoleModel role)
        {
            return securityService.UpdateRole(role);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="RoleID">The role identifier.</param>
        /// <returns></returns>
        public Response<RoleModel> DeleteRole(long RoleID, DateTime modifiedOn)
        {
            return securityService.DeleteRole(RoleID, modifiedOn);
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        public Response<ModuleModel> GetModule()
        {
            return securityService.GetModule();
        }

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModuleModel> AssignModuleToRole(List<RoleModuleModel> role)
        {
            return securityService.AssignModuleToRole(role);
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The role identifier.</param>
        /// <returns></returns>
        public Response<RoleModel> GetRoleById(long id)
        {
            return securityService.GetRoleById(id);
        }

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The role identifier.</param>
        /// <returns></returns>
        public Response<RoleModuleModel> GetModuleByRoleId(long id)
        {
            return securityService.GetModuleByRoleId(id);
        }

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The module identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<PermissionModel> GetAssignedPermissionByModuleId(long id, long roleId)
        {
            return securityService.GetAssignedPermissionByModuleId(id, roleId);
        }

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<PermissionModel> GetAssignedPermissionByFeatureId(long id, long roleId)
        {
            return securityService.GetAssignedPermissionByFeatureId(id, roleId);
        }

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The module identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleFeatureModel> GetFeaturePermissionByModuleId(long id, long roleId)
        {
            return securityService.GetFeaturePermissionByModuleId(id, roleId);
        }

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleFeatureModel> GetFeaturePermissionByRoleId(long roleId)
        {
            return securityService.GetFeaturePermissionByRoleId(roleId);
        }

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleModel> GetModulePermissionByRoleId(long roleId)
        {
            return securityService.GetModulePermissionByRoleId(roleId);
        }

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RolePermissionsModel> AssignRolePermission(RolePermissionsModel role)
        {
            return securityService.AssignRolePermission(role);
        }

        #endregion Role

        #region Role Security

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>
        public Response<RoleSecurityModel> GetUserRoleSecurity()
        {
            return securityService.GetUserRoleSecurity();
        }

        /// <summary>
        /// Verifies the role permission.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="permissionKey">The permission key.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public Response<ScalarResult<bool>> VerifyRolePermission(int userId, string modules, string permissionKey, string permission)
        {
            return securityService.VerifyRolePermission(userId, modules, permissionKey, permission);
        }

        #endregion Role Security

        #region Credential Security

        public Response<CredentialSecurityModel> GetUserCredentialSecurity()
        {
            return securityService.GetUserCredentialSecurity();
        }

        #endregion Credential Security
    }
}