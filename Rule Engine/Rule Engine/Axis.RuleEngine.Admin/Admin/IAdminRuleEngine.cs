using System;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.RuleEngine.Admin
{
    public interface IAdminRuleEngine
    {
        Response<UserModel> GetUsers(string userSch);
        Response<UserModel> AddUser(UserModel user);
        Response<UserModel> UpdateUser(UserModel user);
        Response<UserModel> RemoveUser(int userID, DateTime modifiedOn);
        Response<UserModel> ActivateUser(UserModel user);
        Response<UserModel> UnlockUser(UserModel user);
        Response<UserModel> SendNewUserEmail(UserModel user);
        Response<UserRoleModel> GetUserRoles(int userID);
        Response<UserCredentialModel> GetUserCredentials(int userID);
        Response<UserCredentialModel> AddUserCredential(UserCredentialModel userCredential);
        Response<UserCredentialModel> UpdateUserCredential(UserCredentialModel userCredential);
        Response<UserCredentialModel> DeleteUserCredential(int userCredentialID, DateTime modifiedOn);
        Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword);
    }
}
