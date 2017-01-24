using System;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.Service.Admin;

namespace Axis.RuleEngine.Admin
{
    public class AdminRuleEngine : IAdminRuleEngine
    {
        #region Class Variables

        private IAdminService adminService;

        #endregion

        #region Constructors

        public AdminRuleEngine(IAdminService adminService)
        {
            this.adminService = adminService;
        }

        #endregion

        #region Methods

        public Response<UserModel> GetUsers(string userSch)
        {
            return adminService.GetUsers(userSch);
        }

        public Response<UserModel> AddUser(UserModel user)
        {
            return adminService.AddUser(user);
        }

        public Response<UserModel> UpdateUser(UserModel user)
        {
            return adminService.UpdateUser(user);
        }

        public Response<UserModel> RemoveUser(int userID, DateTime modifiedOn)
        {
            return adminService.RemoveUser(userID, modifiedOn);
        }

        public Response<UserModel> ActivateUser(UserModel user)
        {
            return adminService.ActivateUser(user);
        }

        public Response<UserModel> UnlockUser(UserModel user)
        {
            return adminService.UnlockUser(user);
        }

        public Response<UserModel> SendNewUserEmail(UserModel user)
        {
            return adminService.SendNewUserEmail(user);
        }

        public Response<UserRoleModel> GetUserRoles(int userID)
        {
            return adminService.GetUserRoles(userID);
        }

        public Response<UserCredentialModel> GetUserCredentials(int userID)
        {
            return adminService.GetUserCredentials(userID);
        }

        public Response<UserCredentialModel> AddUserCredential(UserCredentialModel userCredential)
        {
            return adminService.AddUserCredential(userCredential);
        }

        public Response<UserCredentialModel> UpdateUserCredential(UserCredentialModel userCredential)
        {
            return adminService.UpdateUserCredential(userCredential);
        }

        public Response<UserCredentialModel> DeleteUserCredential(int userCredentialID, DateTime modifiedOn)
        {
            return adminService.DeleteUserCredential(userCredentialID, modifiedOn);
        }
        
        public Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            return adminService.VerifySecurityDetails(resetPassword);
        }

        #endregion
    }
}
