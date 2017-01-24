using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Http;
using Axis.Helpers;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.Model.Admin;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Service.Controllers
{
    public class UserDetailController : BaseApiController
    {
        #region Class Variables

        private readonly IUserDetailRuleEngine _userDetailRuleEngine = null;

        #endregion

        #region Constructors

        public UserDetailController(IUserDetailRuleEngine userDetailRuleEngine)
        {
            _userDetailRuleEngine = userDetailRuleEngine;
        }

        #endregion

        #region Public Methods
        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUser(int userID)
        {
            Response<UserModel> responseObject = _userDetailRuleEngine.GetUser(userID);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[]{Permission.Create,Permission.Update})]
        [HttpPost]
        public IHttpActionResult AddUser(UserModel userDetail)
        {
            userDetail.Password = CommonHelper.GenerateRandomString(10);
            Response<UserModel> responseObject = _userDetailRuleEngine.AddUser(userDetail);

            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult UpdateUser(UserModel userDetail)
        {
            Response<UserModel> responseObject = _userDetailRuleEngine.UpdateUser(userDetail);
            return new HttpResult<Response<UserModel>>(responseObject, Request);
        }

        #endregion

        #region Additional user details
        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetCoSignatures(int userID)
        {
            var userResponse = _userDetailRuleEngine.GetCoSignatures(userID);
            return new HttpResult<Response<CoSignaturesModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddCoSignatures(CoSignaturesModel signature)
        {
            var userResponse = _userDetailRuleEngine.AddCoSignatures(signature);
            return new HttpResult<Response<CoSignaturesModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateCoSignatures(CoSignaturesModel signature)
        {
            var userResponse = _userDetailRuleEngine.UpdateCoSignatures(signature);
            return new HttpResult<Response<CoSignaturesModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteCoSignatures(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<CoSignaturesModel>>(_userDetailRuleEngine.DeleteCoSignatures(id, modifiedOn), Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUserIdentifierDetails(int userID)
        {
            var userResponse = _userDetailRuleEngine.GetUserIdentifierDetails(userID);
            return new HttpResult<Response<UserIdentifierDetailsModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            var userResponse = _userDetailRuleEngine.AddUserIdentifierDetails(useridentifier);
            return new HttpResult<Response<UserIdentifierDetailsModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateUserIdentifierDetails(UserIdentifierDetailsModel useridentifier)
        {
            var userResponse = _userDetailRuleEngine.UpdateUserIdentifierDetails(useridentifier);
            return new HttpResult<Response<UserIdentifierDetailsModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteUserIdentifierDetails(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserIdentifierDetailsModel>>(_userDetailRuleEngine.DeleteUserIdentifierDetails(id, modifiedOn), Request);

        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUserAdditionalDetails(int userID)
        {
            var userResponse = _userDetailRuleEngine.GetUserAdditionalDetails(userID);
            return new HttpResult<Response<UserAdditionalDetailsModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            var userResponse = _userDetailRuleEngine.AddUserAdditionalDetails(details);
            return new HttpResult<Response<UserAdditionalDetailsModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateUserAdditionalDetails(UserAdditionalDetailsModel details)
        {
            var userResponse = _userDetailRuleEngine.UpdateUserAdditionalDetails(details);
            return new HttpResult<Response<UserAdditionalDetailsModel>>(userResponse, Request);
        }

        [Authorization(PermissionKeys = new String[] { SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserDetails, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_AdditionalDetails }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteUserAdditionalDetails(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<UserAdditionalDetailsModel>>(_userDetailRuleEngine.DeleteUserAdditionalDetails(id, modifiedOn), Request);

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
            if (!CommonHelper.IsAlphaAndSpacesOnlyWithLengthRange(user.MiddleName, 2, 50))
            {
                sb.Append("MiddleName is not valid. ");
                hasError = true;
            }
            if (!CommonHelper.IsValidEmail(user.PrimaryEmail))
            {
                sb.Append("Invalid email format.");
                hasError = true;
            }

            ValidationChecker checker = new ValidationChecker
            {
                HasError = hasError,
                Message = sb.ToString()
            };

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