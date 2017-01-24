using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Common.User;
using Axis.Model.Account;


namespace Axis.DataProvider.Account.UserSecurity
{
    /// <summary>
    /// Data provider interface
    /// </summary>
    public interface IUserSecurityDataProvider
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
