using System;
using Axis.Model.Common;
using Axis.Model.Security.RoleManagement;
using Axis.Service;
using Axis.Configuration;
using Axis.Helpers;
using System.Collections.Specialized;
using Axis.Model.Security;

namespace Axis.PresentationEngine.Areas.Security.Repository
{
    public class RoleManagementRepository : IRoleManagementRepository
    {
        private readonly CommunicationManager _communicationManager;

        private const string BaseRoute = "RoleManagement/";

        #region Constructors
        public RoleManagementRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public RoleManagementRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }
        #endregion Constructors

        public Response<RoleModuleDetailsModel> GetRoleModuleDetails(long ModuleID)
        {
            var apiUrl = BaseRoute + "GetRoleModuleDetails";
            var param = new NameValueCollection { { "ModuleID", ModuleID.ToString() } };

            return _communicationManager.Get<Response<RoleModuleDetailsModel>>(param, apiUrl);
        }

        public Response<RoleModuleComponentModel> GetRoleModuleComponentDetails(long RoleModuleID)
        {
            var apiUrl = BaseRoute + "GetRoleModuleComponentDetails";
            var param = new NameValueCollection { { "RoleModuleID", RoleModuleID.ToString() } };

            return _communicationManager.Get<Response<RoleModuleComponentModel>>(param, apiUrl);
        }

        public Response<PermissionModel> GetPermissions()
        {
            var apiUrl = BaseRoute + "GetPermissions";

            return _communicationManager.Get<Response<PermissionModel>>(apiUrl);
        }

        public Response<RoleModuleSaveModel> SaveModulePermissions(RoleModuleSaveModel roleModuleSave)
        {
            var apiUrl = BaseRoute + "SaveModulePermissions";

            return _communicationManager.Post<RoleModuleSaveModel, Response<RoleModuleSaveModel>>(roleModuleSave, apiUrl);
        }



    }
}