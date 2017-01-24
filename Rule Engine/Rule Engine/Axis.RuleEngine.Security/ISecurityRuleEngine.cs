using Axis.Model.Common;
using Axis.Model.Security;
using System;
using System.Collections.Generic;

namespace Axis.RuleEngine.Security
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurityRuleEngine
    {
        #region Role

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        Response<RoleModel> GetRoles(string roleName);

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RoleModel> AddRole(RoleModel role);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RoleModel> UpdateRole(RoleModel role);

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="RoleID">The role identifier.</param>
        /// <returns></returns>
        Response<RoleModel> DeleteRole(long RoleID, DateTime modifiedOn);

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        Response<ModuleModel> GetModule();

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RoleModuleModel> AssignModuleToRole(List<RoleModuleModel> role);

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<RoleModel> GetRoleById(long id);

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<RoleModuleModel> GetModuleByRoleId(long id);

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<PermissionModel> GetAssignedPermissionByModuleId(long id, long roleId);

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<PermissionModel> GetAssignedPermissionByFeatureId(long id, long roleId);

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<ModuleFeatureModel> GetFeaturePermissionByModuleId(long id, long roleId);

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<ModuleFeatureModel> GetFeaturePermissionByRoleId(long roleId);

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<ModuleModel> GetModulePermissionByRoleId(long roleId);

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RolePermissionsModel> AssignRolePermission(RolePermissionsModel role);

        #endregion Role

        #region Role Security

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>
        Response<RoleSecurityModel> GetUserRoleSecurity();

        /// <summary>
        /// Verifies the role permission.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="permissionKey">The permission key.</param>
        /// <param name="permission">Name of the action.</param>
        /// <returns></returns>
        Response<ScalarResult<bool>> VerifyRolePermission(int userId, string modules, string permissionKey, string permission);

        #endregion Role Security

        #region Credential Security

        Response<CredentialSecurityModel> GetUserCredentialSecurity();

        #endregion Credential Security
    }
}