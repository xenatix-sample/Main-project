using System;
using System.Linq;
using System.Collections.Generic;
using System.Data.SqlClient;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Security.RoleManagement;
using Axis.Data.Repository.Schema;
using Axis.Model.Security;
using System.IO;
using System.Xml;

namespace Axis.DataProvider.Security
{
    public class RoleManagementDataProvider : IRoleManagementDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork _unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="RoleManagementDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public RoleManagementDataProvider(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        #region RoleManagement

        /// <summary>
        /// Gets the Role Modules.
        /// </summary>
        /// <param name="ModuleID">ID of the module.</param>
        /// <returns></returns>
        public Response<RoleModuleDetailsModel> GetRoleModuleDetails(long ModuleID)
        {
            var repositoryPermissionDetails = _unitOfWork.GetRepository<PermissionModel>(SchemaName.Core);
            var permissionDetailsResult = repositoryPermissionDetails.ExecuteStoredProc("usp_GetPermissionDetails", null);

            var repositoryModuleDetails = _unitOfWork.GetRepository<RoleModuleDetailsModel>(SchemaName.Core);
            var procParams = new List<SqlParameter> { new SqlParameter("RoleModuleID", ModuleID) };
            var roleModuleDetailsResult = repositoryModuleDetails.ExecuteStoredProc("usp_GetRoleModuleDetails", procParams);

            if (roleModuleDetailsResult.ResultCode != 0)
                return roleModuleDetailsResult;

            var RoleModuleID = roleModuleDetailsResult.DataItems[0].RoleModuleID;

            var repositoryModulePermission = _unitOfWork.GetRepository<RoleModulePermissionDetailsModel>(SchemaName.Core);
            var procParamsPermission = new List<SqlParameter> { new SqlParameter("RoleModuleID", RoleModuleID) };
            var roleModulePermissionResult = repositoryModulePermission.ExecuteStoredProc("usp_GetRoleModulePermissionDetails", procParamsPermission);

            roleModuleDetailsResult.DataItems[0].ModulePermissions = new List<RoleModulePermissionDetailsModel>();
            foreach (var item in permissionDetailsResult.DataItems)
            {

                var permission = roleModulePermissionResult.DataItems.FirstOrDefault(x => x.PermissionID == item.PermissionID);
                if (permission != null)
                {
                    roleModuleDetailsResult.DataItems[0].ModulePermissions.Add(permission);
                }
                else
                {
                    var modulePermission = new RoleModulePermissionDetailsModel();
                    modulePermission.PermissionID = item.PermissionID;
                    roleModuleDetailsResult.DataItems[0].ModulePermissions.Add(modulePermission);
                }

            }
            return roleModuleDetailsResult;
        }

        /// <summary>
        /// Gets the Role Modules.
        /// </summary>
        /// <param name="RoleModuleID">ID of the Role module.</param>
        /// <returns></returns>
        public Response<RoleModuleComponentModel> GetRoleModuleComponentDetails(long RoleModuleID)
        {
            var repositoryPermissionDetails = _unitOfWork.GetRepository<PermissionModel>(SchemaName.Core);
            var permissionDetailsResult = repositoryPermissionDetails.ExecuteStoredProc("usp_GetPermissionDetails", null);

            var repositoryModuleComponentDetails = _unitOfWork.GetRepository<RoleModuleComponentDetailsModel>(SchemaName.Core);
            var procParams = new List<SqlParameter> { new SqlParameter("RoleModuleID", RoleModuleID) };
            var roleModuleComponentDetailsResult = repositoryModuleComponentDetails.ExecuteStoredProc("usp_GetRoleModuleComponentDetails", procParams);

            var result = roleModuleComponentDetailsResult.CloneResponse<RoleModuleComponentModel>();
            if (result.ResultCode != 0)
                return result;

            var repositoryModuleComponentPermission = _unitOfWork.GetRepository<RoleModuleComponentPermissionDetailsModel>(SchemaName.Core);
            var procParamsComponentPermission = new List<SqlParameter> { new SqlParameter("RoleModuleID", RoleModuleID) };
            var roleModuleComponentPermissionResult = repositoryModuleComponentPermission.ExecuteStoredProc("usp_GetRoleModuleComponentPermissionDetails", procParamsComponentPermission);

            foreach (var componentDetail in roleModuleComponentDetailsResult.DataItems)
            {
                componentDetail.ModuleComponentPermissions = new List<RoleModuleComponentPermissionDetailsModel>();
                foreach (var item in permissionDetailsResult.DataItems)
                {
                    var permission = roleModuleComponentPermissionResult.DataItems.FirstOrDefault(x => x.PermissionID == item.PermissionID && x.RoleModuleComponentID == componentDetail.RoleModuleComponentID);
                    if (permission != null)
                    {
                        componentDetail.ModuleComponentPermissions.Add(permission);
                    }
                    else
                    {
                        var moduleComponentPermission = new RoleModuleComponentPermissionDetailsModel();
                        moduleComponentPermission.PermissionID = item.PermissionID;
                        componentDetail.ModuleComponentPermissions.Add(moduleComponentPermission);
                    }

                }
            }

            result.DataItems = new List<RoleModuleComponentModel>();

            var groupByRoleModuleComponentDetails = from roleModuleComponentDetail in roleModuleComponentDetailsResult.DataItems
                                                    group roleModuleComponentDetail by roleModuleComponentDetail.FeatureID;

            foreach (var groupRoleModuleComponentDetail in groupByRoleModuleComponentDetails)
            {
                var roleModuleComponent = new RoleModuleComponentModel();

                foreach (var item in groupRoleModuleComponentDetail)
                {
                    roleModuleComponent.FeatureName = item.FeatureName;
                    roleModuleComponent.ModuleComponents.Add(item);
                }
                result.DataItems.Add(roleModuleComponent);
            }
            return result;
        }

        public Response<PermissionModel> GetPermissions()
        {
            var repository = _unitOfWork.GetRepository<PermissionModel>(SchemaName.Core);
            return repository.ExecuteStoredProc("usp_GetPermissionDetails", null);
        }

        public Response<RoleModuleSaveModel> SaveModulePermissions(RoleModuleSaveModel roleModuleSave)
        {
            var repository = _unitOfWork.GetRepository<RoleModuleSaveModel>(SchemaName.Core);
            SqlParameter roleModuleXMLParam = new SqlParameter("RoleModulePermissionsXML", GenerateRequestXml(roleModuleSave));
            roleModuleXMLParam.DbType = System.Data.DbType.Xml;
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", roleModuleSave.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParam = new List<SqlParameter>() { roleModuleXMLParam, modifiedOnParam };
            var result = _unitOfWork.EnsureInTransaction<Response<RoleModuleSaveModel>>(repository.ExecuteNQStoredProc, "usp_SaveRoleModulePermissionDetails", procParam, adonResult: false);

            return result;
        }

        #endregion RoleManagement

        #region Helpers
        private string GenerateRequestXml(RoleModuleSaveModel roleModuleSave)
        {
            using (StringWriter sWriter = new StringWriter())
            {
                XmlWriterSettings settings = new XmlWriterSettings();
                settings.OmitXmlDeclaration = true;

                using (XmlWriter XWriter = XmlWriter.Create(sWriter))
                {
                    XWriter.WriteStartElement("Role");
                    XWriter.WriteStartElement("RoleModule");
                    XWriter.WriteElementString("RoleModuleID", roleModuleSave.roleModule.RoleModuleID.ToString());
                    if(roleModuleSave.roleModule.PermissionLevelID != null)
                        XWriter.WriteElementString("RMPermissionLevelID", roleModuleSave.roleModule.PermissionLevelID.ToString());
                    //<RoleModulePermissions>
                    XWriter.WriteStartElement("RoleModulePermissions");
                    foreach (var modulePermission in roleModuleSave.roleModule.ModulePermissions)
                    {
                        //<RoleModulePermission>
                        XWriter.WriteStartElement("RoleModulePermission");
                        XWriter.WriteElementString("RoleModulePermissionID", modulePermission.RoleModulePermissionID.ToString());
                        if(modulePermission.PermissionLevelID != null)
                            XWriter.WriteElementString("PermissionLevelID", modulePermission.PermissionLevelID.ToString());
                        XWriter.WriteElementString("PermissionID", modulePermission.PermissionID.ToString());
                        //</RoleModulePermission>
                        XWriter.WriteEndElement();
                    }
                    //</RoleModulePermissions>
                    XWriter.WriteEndElement();

                    //<RoleModuleComponents>
                    XWriter.WriteStartElement("RoleModuleComponents");
                    foreach (var roleModuleComponent in roleModuleSave.roleModuleComponents)
                    {
                        foreach (var moduleComponent in roleModuleComponent.ModuleComponents)
                        {
                            //<RoleModuleComponent>
                            XWriter.WriteStartElement("RoleModuleComponent");
                            XWriter.WriteElementString("RoleModuleComponentID", moduleComponent.RoleModuleComponentID.ToString());
                            XWriter.WriteElementString("ModuleComponentID", moduleComponent.ModuleComponentID.ToString());
                            if(moduleComponent.PermissionLevelID != null)
                                 XWriter.WriteElementString("RMCPermissionLevelID", moduleComponent.PermissionLevelID.ToString());
                            //<RoleModuleComponentPermissions>
                            XWriter.WriteStartElement("RoleModuleComponentPermissions");
                            foreach (var moduleComponentPermission in moduleComponent.ModuleComponentPermissions)
                            {
                                XWriter.WriteStartElement("RoleModuleComponentPermission");
                                XWriter.WriteElementString("RoleModuleComponentPermissionID", moduleComponentPermission.RoleModuleComponentPermissionID.ToString());
                                if(moduleComponentPermission.PermissionLevelID != null)
                                    XWriter.WriteElementString("PermissionLevelID", moduleComponentPermission.PermissionLevelID.ToString());
                                XWriter.WriteElementString("PermissionID", moduleComponentPermission.PermissionID.ToString());
                                XWriter.WriteEndElement();
                            }
                            //<RoleModuleComponentPermissions>
                            XWriter.WriteEndElement();
                            //</RoleModuleComponent>
                            XWriter.WriteEndElement();
                        }

                    }
                    //</RoleModuleComponents>
                    XWriter.WriteEndElement();
                    XWriter.WriteEndElement();
                    XWriter.WriteEndElement();
                }

                return sWriter.ToString();
            }
        }

        #endregion Helpers
    }
}
