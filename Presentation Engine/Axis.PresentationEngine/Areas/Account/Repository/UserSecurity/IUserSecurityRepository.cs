using Axis.Model.Account;
using Axis.Model.Admin;
using Axis.Model.Common;
using Axis.PresentationEngine.Areas.Account.Model;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Axis.PresentationEngine.Areas.Account.Respository
{
    /// <summary>
    /// User security repository interface
    /// </summary>
    public interface IUserSecurityRepository
    {
        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        Response<UserSecurityQuestionAnswerViewModel> GetUserSecurityQuestions(int userID);

        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        Task<Response<UserSecurityQuestionAnswerModel>> GetUserSecurityAsync(int userID);

        /// <summary>
        /// Save security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>
        Response<UserSecurityQuestionAnswerViewModel> SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerViewModel> securityQuestions);


        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        Response<UserProfileViewModel> SaveUserPassword(UserProfileViewModel userProfile);

        /// <summary>
        /// Update user signature details
        /// </summary>
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        Response<UserProfileViewModel> UpdateUserSignatureDetails(UserProfileViewModel userProfile);

    }
}