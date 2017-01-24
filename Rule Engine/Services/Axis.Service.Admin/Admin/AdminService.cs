using System.Diagnostics;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Security;
using Axis.Service;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;

namespace Axis.Service.Admin
{
    public class AdminService : IAdminService
    {
        #region Class Variables

        private CommunicationManager communicationManager;
        private const string baseRoute = "admin/";

        #endregion

        #region Constructors

        public AdminService()
        {
            communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Methods

        public Response<UserModel> GetUsers(string userSch)
        {
            var apiUrl = baseRoute + "GetUsers";
            var param = new NameValueCollection();
            param.Add("userSch", string.IsNullOrEmpty(userSch) ? string.Empty : userSch);

            return communicationManager.Get<Response<UserModel>>(param, apiUrl);
        }

        public Response<UserModel> AddUser(UserModel user)
        {
            var apiUrl = baseRoute + "AddUser";
            return communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        public Response<UserModel> UpdateUser(UserModel user)
        {
            var apiUrl = baseRoute + "UpdateUser";
            return communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        public Response<UserModel> RemoveUser(int userID, DateTime modifiedOn)
        {
            var apiUrl = baseRoute + "RemoveUser";
            var param = new NameValueCollection
            {
                {"userID", userID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<UserModel>>(param, apiUrl);
        }

        public Response<UserModel> ActivateUser(UserModel user)
        {
            var apiUrl = baseRoute + "ActivateUser";
            return communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        public Response<UserModel> UnlockUser(UserModel user)
        {
            var apiUrl = baseRoute + "UnlockUser";
            return communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        public Response<UserModel> SendNewUserEmail(UserModel user)
        {
            var apiUrl = baseRoute + "SendNewUserEmail";
            return communicationManager.Post<UserModel, Response<UserModel>>(user, apiUrl);
        }

        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            var apiUrl = baseRoute + "GetUserRoles";
            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());
            return communicationManager.Get<Response<UserRoleModel>>(param, apiUrl);
        }

        public Response<UserCredentialModel> GetUserCredentials(int userID)
        {
            var apiUrl = baseRoute + "GetUserCredentials";
            var param = new NameValueCollection();
            param.Add("userID", userID.ToString());
            return communicationManager.Get<Response<UserCredentialModel>>(param, apiUrl);
        }

        public Response<UserCredentialModel> AddUserCredential(UserCredentialModel userCredential)
        {
            var apiUrl = baseRoute + "AddUserCredential";
            return communicationManager.Post<UserCredentialModel, Response<UserCredentialModel>>(userCredential, apiUrl);
        }

        public Response<UserCredentialModel> UpdateUserCredential(UserCredentialModel userCredential)
        {
            var apiUrl = baseRoute + "UpdateUserCredential";
            var param = new NameValueCollection();
            return communicationManager.Post<UserCredentialModel, Response<UserCredentialModel>>(userCredential, apiUrl);
        }

        public Response<UserCredentialModel> DeleteUserCredential(int userCredentialID, DateTime modifiedOn)
        {
            var apiUrl = baseRoute + "DeleteUserCredential";
            var param = new NameValueCollection
            {
                {"userCredentialID", userCredentialID.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return communicationManager.Delete<Response<UserCredentialModel>>(param, apiUrl);
        }

        public Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            var apiUrl = baseRoute + "verifySecurityDetails";

            return communicationManager.Post<ResetPasswordModel, Response<ResetPasswordModel>>(resetPassword, apiUrl);
        }

        #endregion
    }
}
