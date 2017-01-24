using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Admin;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataProvider.Account;

namespace Axis.DataEngine.Service.Controllers
{

    public class AdminController : BaseApiController
    {
        #region Class Variables

        IAdminDataProvider adminDataProvider = null;
        #endregion

        #region Constructors

        public AdminController(IAdminDataProvider adminDataProvider)
        {
            this.adminDataProvider = adminDataProvider;
        }

        #endregion

        #region Methods

        [HttpGet]
        public IHttpActionResult GetUsers(string userSch)
        {
            return new HttpResult<Response<UserModel>>(adminDataProvider.GetUsers(userSch), Request);
        }

        [HttpPost]
        public IHttpActionResult AddUser(UserModel user)
        {
            return new HttpResult<Response<UserModel>>(adminDataProvider.AddUser(user), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateUser(UserModel user)
        {
            return new HttpResult<Response<UserModel>>(adminDataProvider.UpdateUser(user), Request);
        }

        [HttpDelete]
        public IHttpActionResult RemoveUser(int userID, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserModel>>(adminDataProvider.RemoveUser(userID, modifiedOn), Request);
        }

        [HttpPost]
        public IHttpActionResult ActivateUser(UserModel user)
        {
            return new HttpResult<Response<UserModel>>(adminDataProvider.ActivateUser(user), Request);
        }

        [HttpPost]
        public IHttpActionResult UnlockUser(UserModel user)
        {
            return new HttpResult<Response<UserModel>>(adminDataProvider.UnlockUser(user), Request);
        }

        [HttpGet]
        public IHttpActionResult GetUserRoles(int userID)
        {
            return new HttpResult<Response<UserRoleModel>>(adminDataProvider.GetUserRoles(userID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetUserCredentials(int userID)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminDataProvider.GetUserCredentials(userID), Request);
        }

        [HttpPost]
        public IHttpActionResult AddUserCredential(UserCredentialModel userCredential)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminDataProvider.AddUserCredential(userCredential), Request);
        }

        [HttpPost]
        public IHttpActionResult UpdateUserCredential(UserCredentialModel userCredential)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminDataProvider.UpdateUserCredential(userCredential), Request);
        }

        [HttpDelete]
        public IHttpActionResult DeleteUserCredential(int userCredentialID, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminDataProvider.DeleteUserCredential(userCredentialID, modifiedOn), Request);
        }

        /// <summary>
        /// Handles the sending of the new user's password
        /// </summary>
        /// <param name="user">The new user model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SendNewUserEmail(UserModel user)
        {
            var response = adminDataProvider.SendNewUserEmail(user);
            return new HttpResult<Response<UserModel>>(response, Request);
        }

        /// <summary>
        /// Verifies the security details.
        /// </summary>
        /// <param name="resetPassword">The reset password.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            var verifySecurityDetailsResult = adminDataProvider.VerifySecurityDetails(resetPassword);

            return new HttpResult<Response<ResetPasswordModel>>(verifySecurityDetailsResult, Request);
        }

        #endregion
    }
}