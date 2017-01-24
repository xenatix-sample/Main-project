using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Generic;
using Axis.Model.Common.User;

namespace Axis.Service.Account
{
    /// <summary>
    /// User security service implimentation
    /// </summary>
    public class UserSecurityService : IUserSecurityService
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "userSecurity/";

        #endregion

        #region Constructors

        public UserSecurityService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        public Response<UserSecurityQuestionAnswerModel> GetUserSecurityQuestions(int userID)
        {
            string apiUrl = BaseRoute + "GetUserSecurityQuestions";
            var param = new NameValueCollection { { "userID", userID.ToString() } };

            return _communicationManager.Get<Response<UserSecurityQuestionAnswerModel>>(param, apiUrl);
        }


        /// <summary>
        /// Save security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>
        public Response<UserSecurityQuestionAnswerModel> SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerModel> securityQuestions)
        {
            string apiUrl = BaseRoute + "SaveUserSecurityQuestions";
            return _communicationManager.Post<List<UserSecurityQuestionAnswerModel>, Response<UserSecurityQuestionAnswerModel>>(securityQuestions, apiUrl);
        }

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        public Response<UserProfileModel> SaveUserPassword(UserProfileModel userProfile)
        {
            string apiUrl = BaseRoute + "SaveUserPassword";
            return _communicationManager.Post<UserProfileModel, Response<UserProfileModel>>(userProfile, apiUrl);
        }

        /// <summary>
        /// Update user signature details
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public Response<UserProfileModel> UpdateUserSignatureDetails(UserProfileModel userProfile)
        {
            string apiUrl = BaseRoute + "UpdateUserSignatureDetails";
            return _communicationManager.Post<UserProfileModel, Response<UserProfileModel>>(userProfile, apiUrl);
        }

        #endregion
    }
}
