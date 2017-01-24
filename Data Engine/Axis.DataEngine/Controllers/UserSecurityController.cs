using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Account.UserProfile;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.User;
using System.Collections.Generic;
using Axis.Model.Admin;
using Axis.DataProvider.Account.UserSecurity;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// User Security controller
    /// </summary>
    public class UserSecurityController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// Data provider instance
        /// </summary>
        IUserSecurityDataProvider _userSecurityDataProvider = null;

        #endregion

        #region Constructors

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="userSecurityDataProvider"> Data provider instance</param>
        public UserSecurityController(IUserSecurityDataProvider userSecurityDataProvider)
        {
            _userSecurityDataProvider = userSecurityDataProvider;
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
            return new HttpResult<Response<UserSecurityQuestionAnswerModel>>(_userSecurityDataProvider.GetUserSecurityQuestions(userID), Request);
        }


        /// <summary>
        /// Save security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerModel> securityQuestions)
        {
            return new HttpResult<Response<UserSecurityQuestionAnswerModel>>(_userSecurityDataProvider.SaveUserSecurityQuestions(securityQuestions), Request);
        }

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult SaveUserPassword(UserProfileModel userProfile)
        {
            return new HttpResult<Response<UserProfileModel>>(_userSecurityDataProvider.SaveUserPassword(userProfile), Request);
        }

        /// <summary>
        /// Update user signature password details 
        /// </summary>
        /// <param name="userProfile">user profile model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult UpdateUserSignatureDetails(UserProfileModel userProfile)
        {
            return new HttpResult<Response<UserProfileModel>>(_userSecurityDataProvider.UpdateUserSignatureDetails(userProfile), Request);
        }

        #endregion
    }
}