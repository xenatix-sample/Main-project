using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Xml.Linq;

namespace Axis.DataProvider.Security
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityDataProvider : ISecurityDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private IUnitOfWork unitOfWork = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SecurityDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        #region Role

        /// <summary>
        /// Gets the roles.
        /// </summary>
        /// <param name="roleName">Name of the role.</param>
        /// <returns></returns>
        public Response<RoleModel> GetRoles(string roleName)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RoleModel>();

            SqlParameter roleNameParam = new SqlParameter("RoleName", roleName ?? string.Empty);
            List<SqlParameter> procParams = new List<SqlParameter>() { roleNameParam };

            var rolePermission = modulePermissionRepository.ExecuteStoredProc("usp_GetRole", procParams);

            return rolePermission;
        }

        /// <summary>
        /// Adds the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModel> AddRole(RoleModel role)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RoleModel>();
            //SqlParameter roleIDParam = new SqlParameter("RoleID", role.RoleID);
            SqlParameter nameParam = new SqlParameter("Name", role.Name ?? string.Empty);
            SqlParameter descriptionParam = new SqlParameter("Description", role.Description ?? string.Empty);
            SqlParameter effectiveDate = new SqlParameter("EffectiveDate", role.EffectiveDate ?? DateTime.Now);

            SqlParameter expirationDate;
            if (role.ExpirationDate != null)
                expirationDate = new SqlParameter("ExpirationDate", role.ExpirationDate);
            else
                expirationDate = new SqlParameter("ExpirationDate", DBNull.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", role.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { nameParam, descriptionParam, effectiveDate, expirationDate, modifiedOnParam };

            return modulePermissionRepository.ExecuteNQStoredProc("usp_AddRole", procParams, idResult: true);
        }

        /// <summary>
        /// Updates the role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModel> UpdateRole(RoleModel role)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RoleModel>();
            SqlParameter roleIDParam = new SqlParameter("RoleID", role.RoleID);
            SqlParameter nameParam = new SqlParameter("Name", role.Name ?? string.Empty);
            SqlParameter descriptionParam = new SqlParameter("Description", role.Description ?? string.Empty);
            SqlParameter effectiveDate = new SqlParameter("EffectiveDate", role.EffectiveDate ?? DateTime.Now);
            SqlParameter expirationDate;
            if (role.ExpirationDate != null)
                expirationDate = new SqlParameter("ExpirationDate", role.ExpirationDate);
            else
                expirationDate = new SqlParameter("ExpirationDate", DBNull.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", role.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { roleIDParam, nameParam, descriptionParam, effectiveDate, expirationDate, modifiedOnParam };

            return modulePermissionRepository.ExecuteNQStoredProc("usp_UpdateRole", procParams);
        }

        /// <summary>
        /// Deletes the role.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Response<RoleModel> DeleteRole(long id, DateTime modifiedOn)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RoleModel>();
            SqlParameter roleIDParam = new SqlParameter("RoleID", id);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", modifiedOn);
            List<SqlParameter> procParams = new List<SqlParameter>() { roleIDParam, modifiedOnParam };

            return modulePermissionRepository.ExecuteNQStoredProc("usp_DeleteRole", procParams);
        }

        /// <summary>
        /// Gets the module.
        /// </summary>
        /// <returns></returns>
        public Response<ModuleModel> GetModule()
        {
            var modulePermissionRepository = unitOfWork.GetRepository<ModuleModel>();

            return modulePermissionRepository.ExecuteStoredProc("usp_GetModule", null);
        }

        /// <summary>
        /// Assigns the module to role.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RoleModuleModel> AssignModuleToRole(List<RoleModuleModel> role)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RoleModuleModel>();

            SqlParameter roleModuleParam = new SqlParameter("roleModule", GenerateAssignModuleToRoleXml(role));
            List<SqlParameter> procParams = new List<SqlParameter>() { roleModuleParam };

            return modulePermissionRepository.ExecuteNQStoredProc("usp_AssignModuleToRole", procParams);
        }

        /// <summary>
        /// Gets the role by identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Response<RoleModel> GetRoleById(long id)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RoleModel>();

            SqlParameter roleIdParam = new SqlParameter("RoleId", id);
            List<SqlParameter> procParams = new List<SqlParameter>() { roleIdParam };

            return modulePermissionRepository.ExecuteStoredProc("usp_GetRoleById", procParams);
        }

        /// <summary>
        /// Gets the module by role identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public Response<RoleModuleModel> GetModuleByRoleId(long id)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RoleModuleModel>();

            SqlParameter roleIdParam = new SqlParameter("RoleId", id);
            List<SqlParameter> procParams = new List<SqlParameter>() { roleIdParam };

            var res = modulePermissionRepository.ExecuteStoredProc("usp_GetRoleModuleByRoleID ", procParams);
            return res;
        }

        /// <summary>
        /// Gets the assigned permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<PermissionModel> GetAssignedPermissionByModuleId(long id, long roleId)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<PermissionModel>();

            SqlParameter moduleIdParam = new SqlParameter("ModuleId", id);
            SqlParameter roleIdParam = new SqlParameter("RoleID", roleId);

            List<SqlParameter> procParams = new List<SqlParameter>() { moduleIdParam, roleIdParam };

            var res = modulePermissionRepository.ExecuteStoredProc("usp_GetAssignedPermissionByModuleId", procParams);
            return res;
        }

        /// <summary>
        /// Gets the assigned permission by feature identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<PermissionModel> GetAssignedPermissionByFeatureId(long id, long roleId)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<PermissionModel>();

            SqlParameter featureIdParam = new SqlParameter("FeatureId", id);
            SqlParameter roleIdParam = new SqlParameter("RoleID", roleId);
            List<SqlParameter> procParams = new List<SqlParameter>() { featureIdParam, roleIdParam };

            return modulePermissionRepository.ExecuteStoredProc("usp_GetAssignedPermissionByFeatureId", procParams);
        }

        /// <summary>
        /// Gets the feature permission by module identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleFeatureModel> GetFeaturePermissionByModuleId(long id, long roleId)
        {
            var moduleFeatureRepository = unitOfWork.GetRepository<ModuleFeatureModel>();

            SqlParameter moduleIdParam = new SqlParameter("ModuleId", id);
            List<SqlParameter> procParams = new List<SqlParameter>() { moduleIdParam };

            var moduleFeature = moduleFeatureRepository.ExecuteStoredProc("usp_GetFeatureByModuleId", procParams);

            moduleFeature.DataItems.ForEach(delegate (ModuleFeatureModel module)
            {
                module.Permissions = GetAssignedPermissionByFeatureId(module.FeatureID, roleId).DataItems;
            });

            return moduleFeature;
        }

        /// <summary>
        /// Gets the feature permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleFeatureModel> GetFeaturePermissionByRoleId(long roleId)
        {
            var moduleFeatureRepository = unitOfWork.GetRepository<ModuleFeatureModel>();

            SqlParameter roleIdParam = new SqlParameter("RoleId", roleId);
            List<SqlParameter> procParams = new List<SqlParameter>() { roleIdParam };

            var moduleFeature = moduleFeatureRepository.ExecuteStoredProc("usp_GetFeatureByRoleId", procParams);

            moduleFeature.DataItems.ForEach(delegate (ModuleFeatureModel module)
            {
                module.Permissions = GetAssignedPermissionByFeatureId(module.FeatureID, roleId).DataItems;
            });

            return moduleFeature;
        }

        /// <summary>
        /// Gets the module permission by role identifier.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <returns></returns>
        public Response<ModuleModel> GetModulePermissionByRoleId(long roleId)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<ModuleModel>();

            SqlParameter roleIdParam = new SqlParameter("RoleId", roleId);
            List<SqlParameter> procParams = new List<SqlParameter>() { roleIdParam };

            var res = modulePermissionRepository.ExecuteStoredProc("usp_GetModuleByRoleId", procParams);

            res.DataItems.ForEach(delegate (ModuleModel module)
            {
                module.Permissions = GetAssignedPermissionByModuleId(module.ModuleID, roleId).DataItems;
            });

            return res;
        }

        /// <summary>
        /// Assigns the role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        public Response<RolePermissionsModel> AssignRolePermission(RolePermissionsModel role)
        {
            var modulePermissionRepository = unitOfWork.GetRepository<RolePermissionsModel>();

            SqlParameter roleModuleParam = new SqlParameter("rolePermission", GenerateAssignRolePermission(role));
            List<SqlParameter> procParams = new List<SqlParameter>() { roleModuleParam };

            return modulePermissionRepository.ExecuteNQStoredProc("usp_AssignRolePermission", procParams);
        }

        #region Generate Xml

        /// <summary>
        /// Generates the assign module to role XML.
        /// </summary>
        /// <param name="roles">The roles.</param>
        /// <returns></returns>
        private string GenerateAssignModuleToRoleXml(List<RoleModuleModel> roles)
        {
            var xmlString =
                new XElement("RoleModule",
                    new XAttribute("RoleID", roles.FirstOrDefault().RoleId),
                        new XElement("Modules",
                            from role in roles
                            select
                            role.ModuleId != 0 ?
                            new XElement("Module",
                                new XAttribute("ModuleID", role.ModuleId),
                                new XAttribute("ModifiedOn", role.ModifiedOn ?? DateTime.Now))
                            : null
                            ));
            return xmlString.ToString();
        }

        /// <summary>
        /// Generates the assign role permission.
        /// </summary>
        /// <param name="role">The role.</param>
        /// <returns></returns>
        private string GenerateAssignRolePermission(RolePermissionsModel role)
        {
            var xmlString =
                new XElement("Permissions",
                    new XAttribute("ModifiedOn", role.ModifiedOn ?? DateTime.Now),
                    new XAttribute("RoleID", role.Role.RoleID),
                        new XElement("Modules",
                            from roleModule in role.Modules.GroupBy(mId => mId.ModuleID)
                                                     .Select(grp => grp.First())
                                                     .ToList()
                            select
                            roleModule.Permissions != null ?
                            new XElement("Module",
                                new XAttribute("ModuleID", roleModule.ModuleID),
                                 from permission in roleModule.Permissions
                                 select new XElement("Permission", permission == null ? 0 : permission.PermissionID)
                             )
                            : null),
                            new XElement("Features",
                            from roleFeature in role.Features.GroupBy(eId => eId.FeatureID)
                                                     .Select(grp => grp.First())
                                                     .ToList()
                            select
                            roleFeature.Permissions != null ?
                            new XElement("Feature",
                             new XAttribute("FeatureID", roleFeature.FeatureID),
                                from permission in roleFeature.Permissions
                                select new XElement("Permission", permission.PermissionID)
                             )
                             : null)
                         );
            return xmlString.ToString();
        }

        #endregion Generate Xml

        #endregion Role

        #region Role Security

        /// <summary>
        /// Gets the user role security.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        public Response<RoleSecurityModel> GetUserRoleSecurity(int userID)
        {
            var roleSecurityRepository = unitOfWork.GetRepository<RoleSecurityModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };

            var roleSecurity = roleSecurityRepository.ExecuteStoredProc("usp_GetUserRoleModuleComponentSecurity", procParams);
            var userRoleModulePermission = GetUserRoleModulePermission(userID);
            var userRoleModuleComponentPermission = GetUserRoleModuleComponentPermission(userID);

            var list = (from role in roleSecurity.DataItems
                        select new RoleSecurityModel()
                        {
                            RoleID = role.RoleID,
                            RoleName = role.RoleName,
                            ModuleID = role.ModuleID,
                            ModuleName = role.ModuleName,
                            RoleModuleID = role.RoleModuleID,
                            DataKey = role.DataKey,
                            ComponentID = role.ComponentID,
                            ComponentName = role.ComponentName,
                            RoleModuleComponentID = role.RoleModuleComponentID,
                            ModulePermissions = userRoleModulePermission.DataItems.Where(roleModule => roleModule.RoleModuleID == role.RoleModuleID).ToList(),
                            ComponentPermissions = userRoleModuleComponentPermission.DataItems.Where(moduleComponent => moduleComponent.RoleModuleComponentID == role.RoleModuleComponentID).ToList()
                        }).ToList();

            roleSecurity.DataItems.Clear();
            roleSecurity.DataItems = list;

            return roleSecurity;
        }

        /// <summary>
        /// Verifies the role permission.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="permissionKey">The permission key.</param>
        /// <param name="actionName">Name of the action.</param>
        /// <returns></returns>
        public Response<ScalarResult<bool>> VerifyRolePermission(int userId, string modules, string permissionKey, string permission)
        {
            var permissionRepository = unitOfWork.GetRepository<ScalarResult<bool>>();

            SqlParameter userIDParam = new SqlParameter("UserID", userId);
            SqlParameter moduleIDParam = new SqlParameter("Modules", modules ?? string.Empty);
            SqlParameter rolePermissionKeyIDParam = new SqlParameter("DataKey", permissionKey ?? string.Empty);
            SqlParameter actionIDParam = new SqlParameter("ActionName", permission ?? string.Empty);

            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, moduleIDParam, rolePermissionKeyIDParam, actionIDParam };

            return permissionRepository.ExecuteStoredProc("usp_VerifyRolePermission", procParams);
        }

        /// <summary>
        /// Gets the role module permission.
        /// </summary>
        /// <param name="userID">The role module identifier.</param>
        /// <returns></returns>
        private Response<UserPermissionModel> GetUserRoleModulePermission(long userID)
        {
            var permissionRepository = unitOfWork.GetRepository<UserPermissionModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };

            return permissionRepository.ExecuteStoredProc("usp_GetUserRoleModulePermission", procParams);
        }

        /// <summary>
        /// Gets the role module component permission.
        /// </summary>
        /// <param name="userID">The role module identifier.</param>
        /// <returns></returns>
        private Response<UserPermissionModel> GetUserRoleModuleComponentPermission(long userID)
        {
            var permissionRepository = unitOfWork.GetRepository<UserPermissionModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };

            return permissionRepository.ExecuteStoredProc("usp_GetUserRoleModuleComponentPermission", procParams);
        }

        #endregion Role Security

        #region Credential Security

        public Response<CredentialSecurityModel> GetUserCredentialSecurity(int userID)
        {
            var credentialSecurityRepository = unitOfWork.GetRepository<CredentialSecurityModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };

            var response = credentialSecurityRepository.ExecuteStoredProc("usp_GetCredentialActionTemplateDetails", procParams);

            return response;
        }

        #endregion Credential Security
    }
}
