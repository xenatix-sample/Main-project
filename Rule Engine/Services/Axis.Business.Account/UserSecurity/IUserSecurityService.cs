using Axis.Model.Account;
using Axis.Model.Common;
using Axis.Model.Common.User;
using System.Collections.Generic;

namespace Axis.Service.Account
{
    /// <summary>
    /// User Security interface
    /// </summary>
    public interface IUserSecurityService
    {
        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        Response<UserSecurityQuestionAnswerModel> GetUserSecurityQuestions(int userID);

        /// <summary>
        /// Save security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>
        Response<UserSecurityQuestionAnswerModel> SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerModel> securityQuestions);

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        Response<UserProfileModel> SaveUserPassword(UserProfileModel userProfile);

        /// <summary>
        /// Update user signature details
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        Response<UserProfileModel> UpdateUserSignatureDetails(UserProfileModel userProfile);
    }
}
