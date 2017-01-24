using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Security
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityService : ISecurityService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "security/";

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityService"/> class.
        /// </summary>
        public SecurityService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #region Role

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public Response<RoleModel> GetRoles(string roleName)
        {
            var apiUrl = baseRoute + "getRoles";
            var param = new NameValueCollection();

            param.Add("roleName", roleName ?? string.Empty);

            return communicationManager.Get<Response<RoleModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModel> AddRole(RoleModel role)
        {
            var apiUrl = baseRoute + "addRole";

            return communicationManager.Post<RoleModel, Response<RoleModel>>(role, apiUrl);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModel> UpdateRole(RoleModel role)
        {
            var apiUrl = baseRoute + "updateRole";

            return communicationManager.Post<RoleModel, Response<RoleModel>>(role, apiUrl);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="RoleID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<RoleModel> DeleteRole(long RoleID, DateTime modifiedOn)
        {
            var apiUrl = baseRoute + "deleteRole";
            var param = new NameValueCollection
            {
                {"id", RoleID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            communicationManager.Delete(param, apiUrl);
            return null;
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        public Response<ModuleModel> GetModule()
        {
            var apiUrl = baseRoute + "getModule";

            return communicationManager.Get<Response<ModuleModel>>(apiUrl);
        }

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModuleModel> AssignModuleToRole(List<RoleModuleModel> role)
        {
            var apiUrl = baseRoute + "assignModuleToRole";

            return communicationManager.Post<List<RoleModuleModel>, Response<RoleModuleModel>>(role, apiUrl);
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Response<RoleModel> GetRoleById(long id)
        {
            var apiUrl = baseRoute + "getRoleById";
            var param = new NameValueCollection();

            param.Add("id", id.ToString());

            return communicationManager.Get<Response<RoleModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Response<RoleModuleModel> GetModuleByRoleId(long id)
        {
            var apiUrl = baseRoute + "getModuleByRoleId";
            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            return communicationManager.Get<Response<RoleModuleModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<PermissionModel> GetAssignedPermissionByModuleId(long id, long roleId)
        {
            var apiUrl = baseRoute + "getAssignedPermissionByModuleId";

            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            param.Add("roleId", roleId.ToString());

            return communicationManager.Get<Response<PermissionModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<PermissionModel> GetAssignedPermissionByFeatureId(long id, long roleId)
        {
            var apiUrl = baseRoute + "getAssignedPermissionByFeatureId";

            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            param.Add("roleId", roleId.ToString());

            return communicationManager.Get<Response<PermissionModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleFeatureModel> GetFeaturePermissionByModuleId(long id, long roleId)
        {
            var apiUrl = baseRoute + "getFeaturePermissionByModuleId";

            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            param.Add("roleId", roleId.ToString());

            return communicationManager.Get<Response<ModuleFeatureModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleFeatureModel> GetFeaturePermissionByRoleId(long roleId)
        {
            var apiUrl = baseRoute + "getFeaturePermissionByRoleId";

            var param = new NameValueCollection();

            param.Add("roleId", roleId.ToString());

            return communicationManager.Get<Response<ModuleFeatureModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleModel> GetModulePermissionByRoleId(long roleId)
        {
            var apiUrl = baseRoute + "getModulePermissionByRoleId";

            var param = new NameValueCollection();

            param.Add("roleId", roleId.ToString());

            return communicationManager.Get<Response<ModuleModel>>(param, apiUrl);
        }

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RolePermissionsModel> AssignRolePermission(RolePermissionsModel role)
        {
            var apiUrl = baseRoute + "assignRolePermission";

            return communicationManager.Post<RolePermissionsModel, Response<RolePermissionsModel>>(role, apiUrl);
        }

        #endregion Role

        #region Role Security

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>
        public Response<RoleSecurityModel> GetUserRoleSecurity()
        {
            var apiUrl = baseRoute + "getUserRoleSecurity";

            return communicationManager.Get<Response<RoleSecurityModel>>(apiUrl);
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
            var apiUrl = baseRoute + "verifyRolePermission";
            var param = new NameValueCollection();

            param.Add("userId", userId.ToString());
            param.Add("modules", modules ?? string.Empty);
            param.Add("permissionKey", permissionKey ?? string.Empty);
            param.Add("permission", permission ?? string.Empty);

            return communicationManager.Get<Response<ScalarResult<bool>>>(param, apiUrl);
        }

        #endregion Role Security

        #region Credential Security

        public Response<CredentialSecurityModel> GetUserCredentialSecurity()
        {
            var apiUrl = baseRoute + "getUserCredentialSecurity";

            return communicationManager.Get<Response<CredentialSecurityModel>>(apiUrl);
        }

        #endregion Credential Security
    }
}