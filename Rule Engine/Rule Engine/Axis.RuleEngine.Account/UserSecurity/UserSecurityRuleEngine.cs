
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.User;
using Axis.Service.Account;

namespace Axis.RuleEngine.Account
{
    /// <summary>
    /// Security rule engine implimentation
    /// </summary>
    public class UserSecurityRuleEngine : IUserSecurityRuleEngine
    {
        #region Class Variables

        private readonly IUserSecurityService _userSecurityService;

        #endregion

        #region Constructors

        public UserSecurityRuleEngine(IUserSecurityService userSecurityService)
        {
            _userSecurityService = userSecurityService;
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        Response<UserSecurityQuestionAnswerModel> IUserSecurityRuleEngine.GetUserSecurityQuestions(int userID)
        {
            return _userSecurityService.GetUserSecurityQuestions(userID);
        }


        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        public Response<UserSecurityQuestionAnswerModel> SaveUserSecurityQuestions(System.Collections.Generic.List<UserSecurityQuestionAnswerModel> securityQuestions)
        {
            return _userSecurityService.SaveUserSecurityQuestions(securityQuestions);
        }

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        public Response<UserProfileModel> SaveUserPassword(UserProfileModel userProfile)
        {
            return _userSecurityService.SaveUserPassword(userProfile);
        }


        /// <summary>
        ///  Update user signature details
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public Response<UserProfileModel> UpdateUserSignatureDetails(UserProfileModel userProfile)
        {
            return _userSecurityService.UpdateUserSignatureDetails(userProfile);
        }

        #endregion



    }
}
