using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Security.Model;
using System;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Security.Repository
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurityRepository
    {
        #region Role

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        Response<RoleViewModel> GetRoles(string roleName);

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RoleViewModel> AddRole(RoleViewModel role);

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RoleViewModel> UpdateRole(RoleViewModel role);

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<RoleViewModel> DeleteRole(long id, DateTime modifiedOn);

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        Response<ModuleViewModel> GetModule();

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RoleModuleViewModel> AssignModuleToRole(List<RoleModuleViewModel> role);

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<RoleViewModel> GetRoleById(long id);

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        Response<RoleModuleViewModel> GetModuleByRoleId(long id);

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<PermissionViewModel> GetAssignedPermissionByModuleId(long id, long roleId);

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<PermissionViewModel> GetAssignedPermissionByFeatureId(long id, long roleId);

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<ModuleFeatureViewModel> GetFeaturePermissionByModuleId(long id, long roleId);

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<ModuleFeatureViewModel> GetFeaturePermissionByRoleId(long roleId);

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        Response<ModuleViewModel> GetModulePermissionByRoleId(long roleId);

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        Response<RolePermissionsViewModel> AssignRolePermission(RolePermissionsViewModel role);

        #endregion Role

        #region Credential

        Response<CredentialSecurityViewModel> GetUserCredentialSecurity();
        
        #endregion Credential

        #region Security Implementation

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>
        Response<RoleSecurityViewModel> GetUserRoleSecurity();

        #endregion Security Implementation
    }
}