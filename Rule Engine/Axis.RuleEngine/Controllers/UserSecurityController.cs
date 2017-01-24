using System.Web.Http;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Security;
using Axis.RuleEngine.Admin;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using Axis.Model.Common.User;
using Axis.Model.Admin;
using Axis.RuleEngine.Account;
using Axis.Security;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;
using System;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    /// User Security controller
    /// </summary>
    public class UserSecurityController: BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// Security Rule instance
        /// </summary>
        private readonly IUserSecurityRuleEngine _userSecurityRuleEngine = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userSecurityDataProvider"> Security Rule instance</param>
        public UserSecurityController(IUserSecurityRuleEngine userCredentialRuleEngine)
        {
            _userSecurityRuleEngine = userCredentialRuleEngine;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetUserSecurityQuestions(int userID)
        {
            Response<UserSecurityQuestionAnswerModel> responseObject = _userSecurityRuleEngine.GetUserSecurityQuestions(userID);
            return new HttpResult<Response<UserSecurityQuestionAnswerModel>>(responseObject, Request);
        }


        /// <summary>
        /// Save security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerModel> securityQuestions)
        {
            foreach (var sq in securityQuestions)
            {
                sq.UserID = AuthContext.Auth.User.UserID;
            }
            Response<UserSecurityQuestionAnswerModel> responseObject = _userSecurityRuleEngine.SaveUserSecurityQuestions(securityQuestions);
            return new HttpResult<Response<UserSecurityQuestionAnswerModel>>(responseObject, Request);
        }

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveUserPassword(UserProfileModel userProfile)
        {
            return new HttpResult<Response<UserProfileModel>>(_userSecurityRuleEngine.SaveUserPassword(userProfile), Request);
        }

        /// <summary>
        /// Update user signature password details 
        /// </summary>
        /// <param name="userProfile">user profile model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserSignatureDetails(UserProfileModel userProfile)
        {
            return new HttpResult<Response<UserProfileModel>>(_userSecurityRuleEngine.UpdateUserSignatureDetails(userProfile), Request);
        }

        #endregion
    }
}