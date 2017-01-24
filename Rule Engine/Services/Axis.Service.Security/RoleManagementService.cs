using System;
using Axis.Model.Common;
using Axis.Model.Security.RoleManagement;
using Axis.Configuration;
using Axis.Security;
using System.Collections.Specialized;
using Axis.Model.Security;
using System.Collections.Generic;

namespace Axis.Service.Security
{
    public class RoleManagementService : IRoleManagementService
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "RoleManagement/";

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleManagementService"/> class.
        /// </summary>
        public RoleManagementService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        public Response<RoleModuleDetailsModel> GetRoleModuleDetails(long ModuleID)
        {
            string apiUrl = baseRoute + "GetRoleModuleDetails";
            var param = new NameValueCollection { { "ModuleID", ModuleID.ToString() } };

            return communicationManager.Get<Response<RoleModuleDetailsModel>>(param, apiUrl);
        }

        public Response<RoleModuleComponentModel> GetRoleModuleComponentDetails(long RoleModuleID)
        {
            string apiUrl = baseRoute + "GetRoleModuleComponentDetails";
            var param = new NameValueCollection { { "RoleModuleID", RoleModuleID.ToString() } };

            return communicationManager.Get<Response<RoleModuleComponentModel>>(param, apiUrl);
        }

        public Response<PermissionModel> GetPermissions()
        {
            string apiUrl = baseRoute + "GetPermissions";

            return communicationManager.Get<Response<PermissionModel>>(apiUrl);
        }

        public Response<RoleModuleSaveModel> SaveModulePermissions(RoleModuleSaveModel roleModuleSave)
        {
            string apiUrl = baseRoute + "SaveModulePermissions";
           
            return communicationManager.Post<RoleModuleSaveModel, Response<RoleModuleSaveModel>>(roleModuleSave, apiUrl);
        }
    }
}
