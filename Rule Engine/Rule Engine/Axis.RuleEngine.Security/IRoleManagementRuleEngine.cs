using System;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Model.Security.RoleManagement;

namespace Axis.RuleEngine.Security
{
    public interface IRoleManagementRuleEngine
    {
        /// <summary>
        /// Gets the Role Modules.
        /// </summary>
        /// <param name="ModuleID">ID of the module.</param>
        /// <returns></returns>
        Response<RoleModuleDetailsModel> GetRoleModuleDetails(long ModuleID);

        /// <summary>
        /// Gets the Role Modules.
        /// </summary>
        /// <param name="RoleModuleID">ID of the Role module.</param>
        /// <returns></returns>
        Response<RoleModuleComponentModel> GetRoleModuleComponentDetails(long RoleModuleID);

        Response<PermissionModel> GetPermissions();

        Response<RoleModuleSaveModel> SaveModulePermissions(RoleModuleSaveModel roleModuleSave);
    }
}
