using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Security.Model;
using Axis.PresentationEngine.Areas.Security.Translator;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;


namespace Axis.PresentationEngine.Areas.Security.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityRepository : ISecurityRepository
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
        /// Initializes a new instance of the <see cref="SecurityRepository"/> class.
        /// </summary>
        public SecurityRepository()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public SecurityRepository(string token)
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
       
        public Response<RoleViewModel> GetRoles(string roleName)
        {
            var apiUrl = baseRoute + "getRoles";
            var param = new NameValueCollection();

            param.Add("roleName", roleName ?? string.Empty);

            var response = communicationManager.Get<Response<RoleModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
       
        public Response<RoleViewModel> AddRole(RoleViewModel role)
        {
            string apiUrl = baseRoute + "addRole";
            var response = communicationManager.Post<RoleModel, Response<RoleModel>>(role.ToEntity(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
       
        public Response<RoleViewModel> UpdateRole(RoleViewModel role)
        {
            string apiUrl = baseRoute + "updateRole";
            var response = communicationManager.Post<RoleModel, Response<RoleModel>>(role.ToEntity(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        
        public Response<RoleViewModel> DeleteRole(long id, DateTime modifiedOn)
        {
            var param = new NameValueCollection
            {
                {"id", id.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            string apiUrl = baseRoute + "deleteRole";
            communicationManager.Delete(param, apiUrl);
            return null;
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
       
        public Response<ModuleViewModel> GetModule()
        {
            var apiUrl = baseRoute + "getModule";

            var response = communicationManager.Get<Response<ModuleModel>>(apiUrl);
            var sjdfsjkdghsjd = response.ToModel();
            return response.ToModel();
        }

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
       
        public Response<RoleModuleViewModel> AssignModuleToRole(List<RoleModuleViewModel> role)
        {
            string apiUrl = baseRoute + "assignModuleToRole";

            var roleModel = new List<RoleModuleModel>();

            if (role == null)
            {
                return new Response<RoleModuleViewModel>()
                {
                    ResultCode = -1,
                    ResultMessage = "Select module to assign."
                };
            }

            role.ForEach(delegate(RoleModuleViewModel item)
            {
                roleModel.Add(item.ToEntity());
            });

            var response = communicationManager.Post<List<RoleModuleModel>, Response<RoleModuleModel>>(roleModel, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
      
        public Response<RoleViewModel> GetRoleById(long id)
        {
            var apiUrl = baseRoute + "getRoleById";

            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            var response = communicationManager.Get<Response<RoleModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
      

        public Response<RoleModuleViewModel> GetModuleByRoleId(long id)
        {
            var apiUrl = baseRoute + "getModuleByRoleId";
            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            var response = communicationManager.Get<Response<RoleModuleModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
       
        public Response<PermissionViewModel> GetAssignedPermissionByModuleId(long id, long roleId)
        {
            var apiUrl = baseRoute + "getAssignedPermissionByModuleId";

            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<PermissionModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
       
        public Response<PermissionViewModel> GetAssignedPermissionByFeatureId(long id, long roleId)
        {
            var apiUrl = baseRoute + "getAssignedPermissionByFeatureId";

            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<PermissionModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
     
        public Response<ModuleFeatureViewModel> GetFeaturePermissionByModuleId(long id, long roleId)
        {
            var apiUrl = baseRoute + "getFeaturePermissionByModuleId";

            var param = new NameValueCollection();

            param.Add("id", id.ToString());
            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<ModuleFeatureModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        
        public Response<ModuleFeatureViewModel> GetFeaturePermissionByRoleId(long roleId)
        {
            var apiUrl = baseRoute + "getFeaturePermissionByRoleId";

            var param = new NameValueCollection();

            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<ModuleFeatureModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        
        public Response<ModuleViewModel> GetModulePermissionByRoleId(long roleId)
        {
            var apiUrl = baseRoute + "getModulePermissionByRoleId";

            var param = new NameValueCollection();

            param.Add("roleId", roleId.ToString());

            var response = communicationManager.Get<Response<ModuleModel>>(param, apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
       
        public Response<RolePermissionsViewModel> AssignRolePermission(RolePermissionsViewModel role)
        {
            string apiUrl = baseRoute + "assignRolePermission";
            var response = communicationManager.Post<RolePermissionsModel, Response<RolePermissionsModel>>(role.ToEntity(), apiUrl);
            return response.ToModel();
        }

        #region Security Implementation

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>        
        public Response<RoleSecurityViewModel> GetUserRoleSecurity()
        {
            string apiUrl = baseRoute + "getUserRoleSecurity";
            var response = communicationManager.Get<Response<RoleSecurityModel>>(apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>        
        public Response<CredentialSecurityViewModel> GetUserCredentialSecurity()
        {
            string apiUrl = baseRoute + "getUserCredentialSecurity";
            var response = communicationManager.Get<Response<CredentialSecurityModel>>(apiUrl);
            return response.ToModel();
        }

        #endregion Security Implementation
    }
}