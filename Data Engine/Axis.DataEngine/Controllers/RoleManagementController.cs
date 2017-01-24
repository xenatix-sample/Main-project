using System;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Common;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataProvider.Security;
using Axis.DataEngine.Helpers.Results;
using Axis.Model.Security.RoleManagement;
using Axis.Model.Security;

namespace Axis.DataEngine.Service.Controllers
{
    public class RoleManagementController : BaseApiController
    {
        private IRoleManagementDataProvider _roleManagementDataProvider = null;

        public RoleManagementController(IRoleManagementDataProvider roleManagementDataProvider)
        {
            this._roleManagementDataProvider = roleManagementDataProvider;
        }

        [HttpGet]
        public IHttpActionResult GetRoleModuleDetails(long ModuleID)
        {
            return new HttpResult<Response<RoleModuleDetailsModel>>(_roleManagementDataProvider.GetRoleModuleDetails(ModuleID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetRoleModuleComponentDetails(long RoleModuleID)
        {
            return new HttpResult<Response<RoleModuleComponentModel>>(_roleManagementDataProvider.GetRoleModuleComponentDetails(RoleModuleID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetPermissions()
        {
            return new HttpResult<Response<PermissionModel>>(_roleManagementDataProvider.GetPermissions(), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveModulePermissions(RoleModuleSaveModel roleModuleSave)
        {
            return new HttpResult<Response<RoleModuleSaveModel>>(_roleManagementDataProvider.SaveModulePermissions(roleModuleSave), Request);
        }
        
    }
}