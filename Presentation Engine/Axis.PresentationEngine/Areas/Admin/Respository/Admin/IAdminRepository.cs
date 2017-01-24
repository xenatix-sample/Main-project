using System;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.PresentationEngine.Areas.Account.Model;
using Axis.PresentationEngine.Areas.Admin.Model;

namespace Axis.PresentationEngine.Areas.Admin.Respository
{
    public interface IAdminRepository
    {
        Response<UserViewModel> GetUsers(string userSch);
        Response<UserViewModel> AddUser(UserViewModel user);
        Response<UserViewModel> UpdateUser(UserViewModel user);
        Response<UserViewModel> RemoveUser(UserViewModel user);
        Response<UserViewModel> ActivateUser(UserViewModel user);
        Response<UserViewModel> UnlockUser(UserViewModel user);
        Response<UserRoleModel> GetUserRoles(int userID);
        Response<UserCredentialViewModel> GetUserCredentials(UserViewModel userView);
        Response<UserCredentialViewModel> AddUserCredential(UserCredentialViewModel userView);
        Response<UserCredentialViewModel> UpdateUserCredential(UserCredentialViewModel userView);
        Response<UserCredentialViewModel> DeleteUserCredential(int userCredentialID, DateTime modifiedOn);
        Response<ResetPasswordViewModel> VerifySecurityDetails(ResetPasswordViewModel resetPassword);
    }
}