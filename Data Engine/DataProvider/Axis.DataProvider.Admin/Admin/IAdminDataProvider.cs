using System;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;

namespace Axis.DataProvider.Admin
{
    /// <summary>
    ///
    /// </summary>
    public interface IAdminDataProvider
    {
        /// <summary>
        /// Gets the users.
        /// </summary>
        /// <param name="userSch">The generic search field on the user administration screen.</param>
        /// <returns></returns>
        Response<UserModel> GetUsers(string userSch);

        /// <summary>
        /// Gets the user by email.
        /// </summary>
        /// <param name="email">The email.</param>
        /// <returns></returns>
        Response<UserModel> GetUserByEmail(string email);

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Response<UserModel> AddUser(UserModel user);

        /// <summary>
        /// Updates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Response<UserModel> UpdateUser(UserModel user);

        /// <summary>
        /// Removes the user.
        /// </summary>
        /// <param name="userID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<UserModel> RemoveUser(int userID, DateTime modifiedOn);

        /// <summary>
        /// Activates the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Response<UserModel> ActivateUser(UserModel user);

        /// <summary>
        /// Unlocks the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns></returns>
        Response<UserModel> UnlockUser(UserModel user);

        /// <summary>
        /// Sends an email to the new user
        /// </summary>
        /// <param name="user">The new user model</param>
        /// <returns></returns>
        Response<UserModel> SendNewUserEmail(UserModel user);

        /// <summary>
        /// Gets the user roles.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <returns></returns>
        Response<UserRoleModel> GetUserRoles(int userID);

        /// <summary>
        /// Gets the user credentials.
        /// </summary>
        /// <param name="userID">The user identifier.</param>
        /// <param name="pageNumber">page user is on for the list of user credentials.</param>
        /// <param name="pageCount">how many credentials to show per page.</param>
        /// <returns></returns>
        Response<UserCredentialModel> GetUserCredentials(int userID);

        /// <summary>
        /// Gets the users by credential identifier.
        /// </summary>
        /// <returns></returns>
        Response<UserModel> GetUsersByCredentialId(long credentialID);

        /// <summary>
        /// adds a credential to a user.
        /// </summary>
        /// <param name="userCredential">credential for a user.</param>
        /// <returns></returns>
        Response<UserCredentialModel> AddUserCredential(UserCredentialModel userCredential);

        /// <summary>
        /// updates the credential for a user.
        /// </summary>
        /// <param name="userCredential">credential for a user.</param>
        /// <returns></returns>
        Response<UserCredentialModel> UpdateUserCredential(UserCredentialModel userCredential);

        /// <summary>
        /// soft deletes a credential for a user.
        /// </summary>
        /// <param name="userCredentialID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<UserCredentialModel> DeleteUserCredential(int userCredentialID, DateTime modifiedOn);

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        Response<ResetPasswordModel> VerifySecurityDetails(ResetPasswordModel resetPassword);
    }
}