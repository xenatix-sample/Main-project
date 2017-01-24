using System;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Text;
using System.Web.Http;
using Axis.Security;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Service.Controllers
{
    
    public class AdminController : BaseApiController
    {
        #region Class Variables

        IAdminRuleEngine adminRuleEngine = null;

        #endregion

        #region Constructors

        public AdminController(IAdminRuleEngine adminRuleEngine)
        {
            this.adminRuleEngine = adminRuleEngine;
        }

        #endregion

        #region Methods
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUsers(string userSch)
        {
            try
            {
                //bool hasError = false;
                //StringBuilder sb = new StringBuilder();

                //** To-Do: Update the validation regex to only allow a small subset of characters
                //if (!CommonHelper.IsAlphaNumericWithLengthRange(userSch ?? string.Empty, 0, 100))
                //{
                //    sb.Append("The search field is invalid.");
                //    hasError = true;
                //}

                //if(hasError)
                //{
                //    var validationResponse = new Response<UserModel>() { DataItems = new List<UserModel>(), ResultCode = -1, ResultMessage = sb.ToString() };
                //    return new HttpResult<Response<UserModel>>(validationResponse, Request);
                //}

                int userID = AuthContext.Auth.User.UserID;
                Response<UserModel> responseObject = adminRuleEngine.GetUsers(userSch);
                responseObject.DataItems.Remove(responseObject.DataItems.Find(x => x.UserID == userID));

                return new HttpResult<Response<UserModel>>(responseObject, Request);
            }
            catch
            {
                throw;
            }
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Create)]
        public IHttpActionResult AddUser(UserModel user)
        {
            ValidationChecker validator = VerifySaveFields(user);

            if (validator.HasError)
            {
                var validationResponse = new Response<UserModel>() { DataItems = new List<UserModel>(), ResultCode = -1, ResultMessage = validator.Message };
                return new HttpResult<Response<UserModel>>(validationResponse, Request);
            }

            //We need to generate a random encrypted password when creating a new user
            user.Password = CommonHelper.GenerateRandomString(10);

            Response<UserModel> data = adminRuleEngine.AddUser(user);

            if (data.ResultCode != 0)
            {
                data.ResultMessage = data.ResultMessage.Contains("UNIQUE KEY") ? "This username is not available" : "Error while adding the user";
            }
            else
            {
                //send the new user an email
                try
                {
                    adminRuleEngine.SendNewUserEmail(user);
                }
                catch (Exception exc)
                {
                    _logger.Error(exc);
                    data.ResultCode = 0;
                    data.ResultMessage = "User added successfully. Error while sending the email";
                    data.AdditionalResult = exc.Message;
                }                
            }

            return new HttpResult<Response<UserModel>>(data, Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Update)]
        public IHttpActionResult UpdateUser(UserModel user)
        {
            ValidationChecker validator = VerifySaveFields(user);

            if (validator.HasError)
            {
                var validationResponse = new Response<UserModel>() { DataItems = new List<UserModel>(), ResultCode = -1, ResultMessage = validator.Message };
                return new HttpResult<Response<UserModel>>(validationResponse, Request);
            }

            Response<UserModel> data = adminRuleEngine.UpdateUser(user);

            if (data.ResultCode != 0)
            {
                if (data.ResultMessage.Contains("UNIQUE KEY"))
                    data.ResultMessage = "This username is not available";
                else
                    data.ResultMessage = "Error while updating the user";
            }

            return new HttpResult<Response<UserModel>>(data, Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult RemoveUser(int userID, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserModel>>(adminRuleEngine.RemoveUser(userID, modifiedOn), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Update)]
        public IHttpActionResult ActivateUser(UserModel user)
        {
            return new HttpResult<Response<UserModel>>(adminRuleEngine.ActivateUser(user), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Update)]
        public IHttpActionResult UnlockUser(UserModel user)
        {
            return new HttpResult<Response<UserModel>>(adminRuleEngine.UnlockUser(user), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Read)]
        public IHttpActionResult GetUserRoles(int userID)
        {
            return new HttpResult<Response<UserRoleModel>>(adminRuleEngine.GetUserRoles(userID), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Read)]
        public IHttpActionResult GetUserCredentials(int userID)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminRuleEngine.GetUserCredentials(userID), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Create)]
        public IHttpActionResult AddUserCredential(UserCredentialModel userCredential)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminRuleEngine.AddUserCredential(userCredential), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Update)]
        public IHttpActionResult UpdateUserCredential(UserCredentialModel userCredential)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminRuleEngine.UpdateUserCredential(userCredential), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteUserCredential(int userCredentialID, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserCredentialModel>>(adminRuleEngine.DeleteUserCredential(userCredentialID, modifiedOn), Request);
        }
        
        [HttpPost]
        public IHttpActionResult VerifySecurityDetails(ResetPasswordModel resetPassword)
        {
            return new HttpResult<Response<ResetPasswordModel>>(adminRuleEngine.VerifySecurityDetails(resetPassword), Request);
        }

        #endregion

        #region Private Methods

        private ValidationChecker VerifySaveFields(UserModel user)
        {
            StringBuilder sb = new StringBuilder();
            bool hasError = false;

            if (!CommonHelper.IsAlphaNumericWithLengthRange(user.UserName, 3, 100))
            {
                sb.Append("UserName is not valid. ");
                hasError = true;
            }
            if (!CommonHelper.IsAlphaAndSpacesOnlyWithLengthRange(user.FirstName, 2, 50))
            {
                sb.Append("FirstName is not valid. ");
                hasError = true;
            }
            if (!CommonHelper.IsAlphaAndSpacesOnlyWithLengthRange(user.LastName, 2, 50))
            {
                sb.Append("LastName is not valid. ");
                hasError = true;
            }
            if (!CommonHelper.IsValidEmail(user.PrimaryEmail))
            {
                sb.Append("Invalid email format.");
                hasError = true;
            }

            ValidationChecker checker = new ValidationChecker();
            checker.HasError = hasError;
            checker.Message = sb.ToString();

            return checker;
        }

        #endregion

        #region Structs

        private struct ValidationChecker
        {
            public string Message { get; set; }
            public bool HasError { get; set; }
        };

        #endregion
    }
}