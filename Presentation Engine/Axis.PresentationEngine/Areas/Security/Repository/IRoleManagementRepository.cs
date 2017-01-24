using System;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Model.Security.RoleManagement;

namespace Axis.PresentationEngine.Areas.Security.Repository
{
    public interface IRoleManagementRepository
    {
        Response<RoleModuleDetailsModel> GetRoleModuleDetails(long ModuleID);

        Response<RoleModuleComponentModel> GetRoleModuleComponentDetails(long RoleModuleID);

        Response<PermissionModel> GetPermissions();

        Response<RoleModuleSaveModel> SaveModulePermissions(RoleModuleSaveModel roleModuleSave);
    }
}
