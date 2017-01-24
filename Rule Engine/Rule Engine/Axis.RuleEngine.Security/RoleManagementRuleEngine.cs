using System;
using Axis.Model.Common;
using Axis.Model.Security.RoleManagement;
using Axis.Service.Security;
using Axis.Model.Security;

namespace Axis.RuleEngine.Security
{
    public class RoleManagementRuleEngine : IRoleManagementRuleEngine
    {
        /// <summary>
        /// The security service
        /// </summary>
        private IRoleManagementService roleManagementService;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityRuleEngine"/> class.
        /// </summary>
        /// <param name="securityService">The security service.</param>
        public RoleManagementRuleEngine(IRoleManagementService roleManagementService)
        {
            this.roleManagementService = roleManagementService;
        }
        public Response<RoleModuleDetailsModel> GetRoleModuleDetails(long ModuleID)
        {
            return roleManagementService.GetRoleModuleDetails(ModuleID);
        }

        public Response<RoleModuleComponentModel> GetRoleModuleComponentDetails(long RoleModuleID)
        {
            return roleManagementService.GetRoleModuleComponentDetails(RoleModuleID);
        }

        public Response<PermissionModel> GetPermissions()
        {
            return roleManagementService.GetPermissions();
        }

        public Response<RoleModuleSaveModel> SaveModulePermissions(RoleModuleSaveModel roleModuleSave)
        {
            return roleManagementService.SaveModulePermissions(roleModuleSave);
        }
    }
}
