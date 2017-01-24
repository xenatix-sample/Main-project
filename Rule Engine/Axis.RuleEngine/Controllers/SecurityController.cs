using Axis.Constant;
using Axis.Logging;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Security;
using System;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
   
    public class SecurityController : BaseApiController
    {
        /// <summary>
        /// The security rule engine
        /// </summary>
        private ISecurityRuleEngine securityRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityController"/> class.
        /// </summary>
        /// <param name="securityRuleEngine">The security rule engine.</param>
        public SecurityController(ISecurityRuleEngine securityRuleEngine)
        {
            this.securityRuleEngine = securityRuleEngine;
        }

        #region Role

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetRoles(string roleName)
        {
            return new HttpResult<Response<RoleModel>>(securityRuleEngine.GetRoles(roleName), Request);
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddRole(RoleModel role)
        {
            return new HttpResult<Response<RoleModel>>(securityRuleEngine.AddRole(role), Request);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult UpdateRole(RoleModel role)
        {
            return new HttpResult<Response<RoleModel>>(securityRuleEngine.UpdateRole(role), Request);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteRole(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<RoleModel>>(securityRuleEngine.DeleteRole(id, modifiedOn), Request);
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetModule()
        {
            return new HttpResult<Response<ModuleModel>>(securityRuleEngine.GetModule(), Request);
        }

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permissions = new String[]{Permission.Create,Permission.Update})]
        [HttpPost]
        public IHttpActionResult AssignModuleToRole(List<RoleModuleModel> role)
        {
            return new HttpResult<Response<RoleModuleModel>>(securityRuleEngine.AssignModuleToRole(role), Request);
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetRoleById(long id)
        {
            return new HttpResult<Response<RoleModel>>(securityRuleEngine.GetRoleById(id), Request);
        }

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetModuleByRoleId(long id)
        {
            return new HttpResult<Response<RoleModuleModel>>(securityRuleEngine.GetModuleByRoleId(id), Request);
        }

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAssignedPermissionByModuleId(long id, long roleId)
        {
            return new HttpResult<Response<PermissionModel>>(securityRuleEngine.GetAssignedPermissionByModuleId(id, roleId), Request);
        }

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAssignedPermissionByFeatureId(long id, long roleId)
        {
            return new HttpResult<Response<PermissionModel>>(securityRuleEngine.GetAssignedPermissionByFeatureId(id, roleId), Request);
        }

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetFeaturePermissionByModuleId(long id, long roleId)
        {
            return new HttpResult<Response<ModuleFeatureModel>>(securityRuleEngine.GetFeaturePermissionByModuleId(id, roleId), Request);
        }

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetFeaturePermissionByRoleId(long roleId)
        {
            return new HttpResult<Response<ModuleFeatureModel>>(securityRuleEngine.GetFeaturePermissionByRoleId(roleId), Request);
        }

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetModulePermissionByRoleId(long roleId)
        {
            return new HttpResult<Response<ModuleModel>>(securityRuleEngine.GetModulePermissionByRoleId(roleId), Request);
        }

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_RoleManagement_RoleDetails, Permission = Permission.Update)]
        [HttpPost]
        public IHttpActionResult AssignRolePermission(RolePermissionsModel role)
        {
            return new HttpResult<Response<RolePermissionsModel>>(securityRuleEngine.AssignRolePermission(role), Request);
        }

        #endregion Role

        #region Role Security

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserRoleSecurity()
        {
            return new HttpResult<Response<RoleSecurityModel>>(securityRuleEngine.GetUserRoleSecurity(), Request);
        }

        #endregion Role Security

        #region Credential Security

        /// <summary>
        /// Gets the user credential security.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SkipLogActionFilter]
        public IHttpActionResult GetUserCredentialSecurity()
        {
            return new HttpResult<Response<CredentialSecurityModel>>(securityRuleEngine.GetUserCredentialSecurity(), Request);
        }

        #endregion Credential Security

    }
}