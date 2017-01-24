using System;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Security;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Results;
using Axis.Model.Common;
using Axis.Model.Security.RoleManagement;
using Axis.Model.Security;

namespace Axis.RuleEngine.Service.Controllers
{
    public class RoleManagementController : BaseApiController
    {
        /// <summary>
        /// The security rule engine
        /// </summary>
        private IRoleManagementRuleEngine roleManagementRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityController"/> class.
        /// </summary>
        /// <param name="securityRuleEngine">The security rule engine.</param>
        public RoleManagementController(IRoleManagementRuleEngine roleManagementRuleEngine)
        {
            this.roleManagementRuleEngine = roleManagementRuleEngine;
        }

        [HttpGet]
        public IHttpActionResult GetRoleModuleDetails(long ModuleID)
        {
            return new HttpResult<Response<RoleModuleDetailsModel>>(roleManagementRuleEngine.GetRoleModuleDetails(ModuleID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetRoleModuleComponentDetails(long RoleModuleID)
        {
            return new HttpResult<Response<RoleModuleComponentModel>>(roleManagementRuleEngine.GetRoleModuleComponentDetails(RoleModuleID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetPermissions()
        {
            return new HttpResult<Response<PermissionModel>>(roleManagementRuleEngine.GetPermissions(), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveModulePermissions(RoleModuleSaveModel roleModuleSave)
        {
            return new HttpResult<Response<RoleModuleSaveModel>>(roleManagementRuleEngine.SaveModulePermissions(roleModuleSave), Request);
        }
    }
}