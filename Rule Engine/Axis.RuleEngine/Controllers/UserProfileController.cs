using System;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.RuleEngine.Account.UserProfile;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using Axis.Constant;

namespace Axis.RuleEngine.Service.Controllers
{
    public class UserProfileController : BaseApiController
    {
        #region Class Variables

        readonly IUserProfileRuleEngine _userProfileRuleEngine = null;

        #endregion

        #region Constructors

        public UserProfileController(IUserProfileRuleEngine userProfileRuleEngine)
        {
            _userProfileRuleEngine = userProfileRuleEngine;
        }

        #endregion

        #region Site Admin Public Methods
        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUserProfile()
        {
            int userID = AuthContext.Auth.User.UserID;
            return new HttpResult<Response<UserProfileModel>>(_userProfileRuleEngine.GetUserProfile(userID), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetUserProfileByID(int userID)
        {
            return new HttpResult<Response<UserProfileModel>>(_userProfileRuleEngine.GetUserProfile(userID), Request);
        }

        [Authorization(PermissionKey = SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_UserProfile, Permissions = new String[]{Permission.Create,Permission.Update})]
        [HttpPost]
        public IHttpActionResult SaveUserProfile(UserProfileModel userProfile)
        {
            if (userProfile.Emails.Count == 0 || (userProfile.Emails.Count == 1 && String.IsNullOrEmpty(userProfile.Emails[0].Email)))
            {
                var errorResponse = new Response<UserProfileModel>() { ResultCode = -1, ResultMessage = "The email address must be provided", DataItems = new List<UserProfileModel>() };
                return new HttpResult<Response<UserProfileModel>>(errorResponse, Request);
            }

            var response = _userProfileRuleEngine.SaveUserProfile(userProfile);
            if (response.ResultCode != 0 && !response.ResultMessage.Contains("password could not be verified"))
            {
                response.ResultMessage = "Error while saving data";
            }

            return new HttpResult<Response<UserProfileModel>>(response, Request);
        }

        #endregion Site Admin Public Methods

        #region My Profile Public Methods
        [HttpGet]
        public IHttpActionResult GetMyProfile()
        {
            int userID = AuthContext.Auth.User.UserID;
            return new HttpResult<Response<UserProfileModel>>(_userProfileRuleEngine.GetUserProfile(userID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetMyProfileByID(int userID)
        {
            return new HttpResult<Response<UserProfileModel>>(_userProfileRuleEngine.GetUserProfile(userID), Request);
        }

        [HttpPost]
        public IHttpActionResult SaveMyProfile(UserProfileModel userProfile)
        {
            if (userProfile.Emails.Count == 0 || (userProfile.Emails.Count == 1 && String.IsNullOrEmpty(userProfile.Emails[0].Email)))
            {
                var errorResponse = new Response<UserProfileModel>() { ResultCode = -1, ResultMessage = "The email address must be provided", DataItems = new List<UserProfileModel>() };
                return new HttpResult<Response<UserProfileModel>>(errorResponse, Request);
            }

            var response = _userProfileRuleEngine.SaveUserProfile(userProfile);
            if (response.ResultCode != 0 && !response.ResultMessage.Contains("password could not be verified"))
            {
                response.ResultMessage = "Error while saving data";
            }

            return new HttpResult<Response<UserProfileModel>>(response, Request);
        }

        #endregion My Profile Public Methods
    }
}