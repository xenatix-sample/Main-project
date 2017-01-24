using System;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Areas.Security.Repository;
using Axis.Model.Common;
using Axis.Model.Security.RoleManagement;
using Axis.Model.Security;
using System.Web.Http;

namespace Axis.PresentationEngine.Areas.Security.ApiControllers
{
    public class RoleManagementController : BaseApiController
    {
        #region Class Variables
        private readonly IRoleManagementRepository _roleManagementRepository;
        #endregion

        #region Constructors

        public RoleManagementController(IRoleManagementRepository roleManagementRepository)
        {
            _roleManagementRepository = roleManagementRepository;
        }

        #endregion

        #region Public Methods
        [HttpGet]
        public Response<RoleModuleDetailsModel> GetRoleModuleDetails(long ModuleID)
        {
            return _roleManagementRepository.GetRoleModuleDetails(ModuleID);
        }

        [HttpGet]
        public Response<RoleModuleComponentModel> GetRoleModuleComponentDetails(long RoleModuleID)
        {
            return _roleManagementRepository.GetRoleModuleComponentDetails(RoleModuleID);
        }

        [HttpGet]
        public Response<PermissionModel> GetPermissions()
        {
            return _roleManagementRepository.GetPermissions();
        }

        [HttpPost]
        public Response<RoleModuleSaveModel> SaveModulePermissions(RoleModuleSaveModel roleModuleSave)
        {
            var result = _roleManagementRepository.SaveModulePermissions(roleModuleSave);
            ClearCache(result);
            return result;
        }
        #endregion

    }
}
