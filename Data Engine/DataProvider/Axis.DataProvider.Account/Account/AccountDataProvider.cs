using Axis.Data.Repository;
using Axis.DataProvider.Admin;
using Axis.DataProvider.SynchronizationService;
using Axis.Logging;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.DirectoryServices;
using System.IO;
using System.Linq;
using System.Xml;

namespace Axis.DataProvider.Account
{
    public class AccountDataProvider : IAccountDataProvider
    {
        private IUnitOfWork _unitOfWork = null;

        protected ILogger _logger;

        public AccountDataProvider(IUnitOfWork unitOfWork, ILogger logger)
        {
            _unitOfWork = unitOfWork;
            _logger = logger;
        }

        /// <summary>
        /// This function helps to authenticate the AD Users 
        /// </summary>
        /// <param name="username"></param>
        /// <param name="pwd"></param>
        /// <returns></returns>
        public int IsAuthenticated(string username, string pwd)
        {
            int statusCode = -6;
            var syncServ = new SynchronizationServiceDataProvider(_logger);

            var path = syncServ.GetServiceConfiguration("ActiveDirectoryPath").DataItems[0].ConfigXML;
            var groupNameconfig = syncServ.GetServiceConfiguration("ActiveDirectoryGroupName").DataItems[0].ConfigXML;


            DirectoryEntry entry = new DirectoryEntry(path, username, pwd);
            if(entry == null)
            {
                statusCode = -7;
                return statusCode;
            }
            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username.Split('\\')[1] + ")";

                // Instead of checking for the groups below this will be a faster approach to check for the group. It will have to wait for a bit.
                // search.Filter = "(&(objectClass = user)(sAMAccountName = " + username.Split('\\')[1] + ")" + ApplicationSettings.LDAPUserGroup;
                // Check for Group Currently we are loading the common name properties which points to the group.
                // Group Objects: cn https://msdn.microsoft.com/en-us/library/ms676913(v=vs.85).aspx
                // CN = Common Name //OU = Organizational Unit //DC = Domain Component

                search.PropertiesToLoad.Add("cn");
                search.PropertiesToLoad.Add("memberOf");

                SearchResult result = search.FindOne();

                if (result == null)
                {
                    // Set A message to display that User Does not exist
                    statusCode = -6;
                    return statusCode;
                }

                int groupCount = result.Properties["memberOf"].Count;

                if (groupCount < 1 || (string)result.Properties["memberOf"][0] == "No Group")
                {
                    // Set A message to display that User Does not belong to a group
                    statusCode = -6;
                    return statusCode;
                }

                for (int groupCounter = 0; groupCounter < groupCount; groupCounter++)
                {
                    if (((string)result.Properties["memberOf"][groupCounter]).Equals(groupNameconfig.ToString(),StringComparison.CurrentCultureIgnoreCase))
                    {
                        statusCode = 1;
                        return statusCode;
                    }
                }
            }
            catch (Exception ex)
            {
                //There is no Exception Method but an Error Method.
                _logger.Error("Active Directory Communication is failing", ex);
                statusCode = -5;
            }

            return statusCode;
        }

        /// <summary>
        /// This function helps application to validate users.
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        public Response<UserModel> Authenticate(UserModel user)
        {
            var userRepository = _unitOfWork.GetRepository<UserModel>();

            var adAuthenticated = 0;

            if (user.UserName.Contains("\\"))
            {
                adAuthenticated = IsAuthenticated(user.UserName, user.Password);
                user.UserName = user.UserName.Split('\\')[1];
            }

            SqlParameter userNameParam = new SqlParameter("UserName", user.UserName ?? (object)DBNull.Value);
            SqlParameter passwordParam = new SqlParameter("Password", user.Password ?? (object)DBNull.Value);
            SqlParameter adAuthenticatedParam = new SqlParameter("ADAuthenticated", adAuthenticated);
            List<SqlParameter> procParams = new List<SqlParameter>() { userNameParam, passwordParam, adAuthenticatedParam };

            var userInfo = userRepository.ExecuteStoredProc("usp_AuthenticateUser", procParams);

            //get all of the user's roles if authenticated
            if (userInfo != null && userInfo.DataItems.Count > 0)
            {
                var userRoleRepository = _unitOfWork.GetRepository<UserRoleModel>();
                SqlParameter userIDParam = new SqlParameter("UserID", userInfo.DataItems.FirstOrDefault().UserID);
                List<SqlParameter> userRoleProcParams = new List<SqlParameter>() { userIDParam };

                var userRoleData = userRoleRepository.ExecuteStoredProc("usp_GetUserRoles", userRoleProcParams);
                if (userRoleData != null && userRoleData.DataItems.Count > 0)
                {
                    List<RoleModel> roles = userRoleData.DataItems.Select(roleModel => new RoleModel()
                    {
                        RoleID = roleModel.RoleID,
                        Name = roleModel.Name,
                        Description = roleModel.Description
                    }).ToList();

                    userInfo.DataItems.FirstOrDefault().Roles = roles;
                }
            }

            return userInfo;
        }

        public Response<UserModel> SetLoginData(UserModel user)
        {
            var userRepository = _unitOfWork.GetRepository<UserModel>();
            //In case of AD user we need to remove the domain to set the Login Data
            if (user.UserName.Contains("\\"))
            {
                user.UserName = user.UserName.Split('\\')[1];
            }


            SqlParameter userIDParam = new SqlParameter("UserID", user.UserID);
            SqlParameter userNameParam = new SqlParameter("UserName", user.UserName ?? (object)DBNull.Value);
            SqlParameter passwordParam = new SqlParameter("Password", user.Password ?? (object)DBNull.Value);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", user.ModifiedOn ?? DateTime.Now);
            SqlParameter modifiedByParam = new SqlParameter("ModifiedBy", user.ModifiedBy);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, userNameParam, passwordParam, modifiedOnParam };
            //List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, userNameParam, passwordParam };

            var loginUpdateInfo = userRepository.ExecuteNQStoredProc("usp_SetLoginData", procParams);
            return loginUpdateInfo;
        }

        public void SyncUser(UserModel user)
        {
            var userRepository = _unitOfWork.GetRepository<UserModel>();

            SqlParameter userNameParam = new SqlParameter("UserName", user.UserName);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", user.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userNameParam, modifiedOnParam };
            userRepository.ExecuteNQStoredProc("usp_SyncUser", procParams);
        }

        public UserModel GetValidUserInfoByToken(AccessTokenModel accessToken)
        {
            var userRepository = _unitOfWork.GetRepository<UserModel>();
            accessToken.Token = accessToken.Token.Replace(" ", "+");

            SqlParameter userNameParam = new SqlParameter("UserName", accessToken.UserName);
            SqlParameter tokenParam = new SqlParameter("Token", accessToken.Token);
            SqlParameter clientIPParam = new SqlParameter("ClientIP", accessToken.ClientIP);
            List<SqlParameter> procParams = new List<SqlParameter>() { userNameParam, tokenParam, clientIPParam };
            var tokenInfo = userRepository.ExecuteStoredProc("usp_GetAccessTokenInfo", procParams).DataItems.FirstOrDefault();

            return tokenInfo;
        }

        public void LogAccessToken(AccessTokenModel accessToken)
        {
            var accessTokenRepository = _unitOfWork.GetRepository<AccessTokenModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", accessToken.UserId);
            SqlParameter userNameParam = new SqlParameter("UserName", accessToken.UserName);
            SqlParameter tokenParam = new SqlParameter("Token", accessToken.Token);
            SqlParameter clientIPParam = new SqlParameter("ClientIP", accessToken.ClientIP);
            SqlParameter issueOnParam = new SqlParameter("GeneratedOn", accessToken.GeneratedOn);
            SqlParameter expireOnParam = new SqlParameter("ExpirationDate", accessToken.ExpirationDate);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", accessToken.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam, userNameParam, tokenParam, clientIPParam, issueOnParam, expireOnParam, modifiedOnParam };
            accessTokenRepository.ExecuteStoredProc("usp_AddAccessToken", procParams);
        }

        public Response<ServerResourceModel> IsValidServerIP(string IPAddress)
        {
            var serverResourceRepository = _unitOfWork.GetRepository<ServerResourceModel>();
            SqlParameter ipAddress = new SqlParameter("IPAddress", IPAddress);
            List<SqlParameter> procParams = new List<SqlParameter>() { ipAddress };
            var serverResourceInfo = serverResourceRepository.ExecuteNQStoredProc("usp_ValidateServerIPAddress", procParams, idResult: true);
            return serverResourceInfo;
        }

        public Response<AccessTokenModel> GetTokenIssueExpireDate()
        {
            var tokenIssueExpireDateRepository = _unitOfWork.GetRepository<AccessTokenModel>();
            var tokenIssueExpireDateInfo = tokenIssueExpireDateRepository.ExecuteStoredProc("usp_GetTokenIssueExpireDate");
            return tokenIssueExpireDateInfo;
        }

        public Response<NavigationModel> GetNavigationItems(int userID)
        {
            var navigationRepository = _unitOfWork.GetRepository<NavigationModel>();
            UserCredentialDataProvider userCredential = new UserCredentialDataProvider(_unitOfWork);
            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = navigationRepository.ExecuteStoredProc("usp_GetNavigationItems", procParams);
            if (result != null && result.DataItems != null && result.DataItems.Count > 0)
            {
                result.DataItems.FirstOrDefault().UserOrganizationStructures = GetOrganizationStructureByUser(userID).DataItems;
                result.DataItems.FirstOrDefault().UserCredentials = userCredential.GetUserCredentials(userID).DataItems.Where(r => ((r.LicenseIssueDate == null || r.LicenseIssueDate <= DateTime.Now.Date) &&
                                                                                                                    (r.LicenseExpirationDate == null || r.LicenseExpirationDate >= DateTime.Now.Date))).ToList();
            }

            return result;
        }

        public Response<UserOrganizationStructureModel> GetOrganizationStructureByUser(int userID)
        {
            var navigationRepository = _unitOfWork.GetRepository<UserOrganizationStructureModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = navigationRepository.ExecuteStoredProc("usp_GetOrgStructureDetailsByUserID", procParams);

            return result;
        }
    }
}
