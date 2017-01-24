using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Linq;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Common.User;
using Axis.Model.Account;

namespace Axis.DataProvider.Account.UserSecurity
{
    /// <summary>
    /// Dataprovider implimentation
    /// </summary>
    public class UserSecurityDataProvider : IUserSecurityDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public UserSecurityDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        /// <summary>
        /// Get User security questions
        /// </summary>
        /// <param name="userID">user id</param>
        /// <returns></returns>
        public Response<UserSecurityQuestionAnswerModel> GetUserSecurityQuestions(int userID)
        {
            var userProfileRepository = _unitOfWork.GetRepository<UserSecurityQuestionAnswerModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = userProfileRepository.ExecuteStoredProc("usp_GetUserSecurityQuestions", procParams);

            return result;
        }

        /// <summary>
        /// Save security questions
        /// </summary>
        /// <param name="securityQuestions">security question and answer object</param>
        /// <returns></returns>
        public Response<UserSecurityQuestionAnswerModel> SaveUserSecurityQuestions(List<UserSecurityQuestionAnswerModel> securityQuestions)
        {
            var userSecurityQuestionRepository = _unitOfWork.GetRepository<UserSecurityQuestionAnswerModel>();
            var userSecurityQuestionResult = new Response<UserSecurityQuestionAnswerModel>() { ResultCode = 0 };

            var questionsToAdd = securityQuestions.Where(questions => questions.UserSecurityQuestionID == 0).ToList();
            var questionsToUpdate = securityQuestions.Where(questions => questions.UserSecurityQuestionID != 0).ToList();

            if (questionsToAdd.Count > 0)
            {
                SqlParameter userIDParam = new SqlParameter("UserID", securityQuestions.FirstOrDefault().UserID);
                var requestXmlValueParam = new SqlParameter("QuestionXml", QuestionsToXml(questionsToAdd));
                var securityQuestionParameters = new List<SqlParameter>() { userIDParam, requestXmlValueParam };
                userSecurityQuestionResult = _unitOfWork.EnsureInTransaction<Response<UserSecurityQuestionAnswerModel>>(userSecurityQuestionRepository.ExecuteNQStoredProc, "usp_AddUserSecurityQuestions", securityQuestionParameters, forceRollback: questionsToAdd.Any(x => x.ForceRollback.GetValueOrDefault(false)));

                if (userSecurityQuestionResult.ResultCode != 0)
                {
                    return userSecurityQuestionResult;
                }
            }

            if (questionsToUpdate.Count > 0)
            {
                var requestXmlValueParam = new SqlParameter("QuestionXml", QuestionsToXml(questionsToUpdate));
                requestXmlValueParam.DbType = DbType.Xml;
               
                var securityQuestionParameters = new List<SqlParameter>() { requestXmlValueParam };
               
                userSecurityQuestionResult = _unitOfWork.EnsureInTransaction<Response<UserSecurityQuestionAnswerModel>>(userSecurityQuestionRepository.ExecuteNQStoredProc, "usp_UpdateUserSecurityQuestions", securityQuestionParameters, forceRollback: questionsToAdd.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return userSecurityQuestionResult;
        }

        /// <summary>
        /// Save User password 
        /// </summary>
        /// <param name="userPassowrd">user password model</param>
        /// <returns></returns>
        public Response<UserProfileModel> SaveUserPassword(UserProfileModel userProfile)
        {
            var response = new Response<UserProfileModel>() { ResultCode = -1, ResultMessage = "Error while saving the user's profile" };
            userProfile.NewPassword = userProfile.NewPassword ?? string.Empty;
            userProfile.CurrentPassword = userProfile.CurrentPassword ?? string.Empty;
            var updatePassword = userProfile.IsTemporaryPassword || userProfile.CurrentPassword != string.Empty;

            var userProfileRepository = _unitOfWork.GetRepository<UserProfileModel>();

            SqlParameter userIdParam = new SqlParameter("UserID", userProfile.UserID);
            SqlParameter updatePasswordParam = new SqlParameter("UpdatePassword", updatePassword);
            SqlParameter newPasswordParam = new SqlParameter("NewPassword", userProfile.NewPassword);
            SqlParameter currentPasswordParam = new SqlParameter("CurrentPassword", userProfile.CurrentPassword);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", userProfile.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIdParam, updatePasswordParam, newPasswordParam, currentPasswordParam, modifiedOnParam };
            
            return _unitOfWork.EnsureInTransaction(userProfileRepository.ExecuteNQStoredProc, "usp_SaveUserProfile", procParams,
                            forceRollback: userProfile.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// Update user signature details
        /// </summary>
        /// <param name="userProfile"></param>
        /// <returns></returns>
        public Response<UserProfileModel> UpdateUserSignatureDetails(UserProfileModel userProfile)
        {
            var response = new Response<UserProfileModel>() { ResultCode = -1, ResultMessage = "Error while saving the user's profile" };
            userProfile.NewDigitalPassword = userProfile.NewDigitalPassword ?? string.Empty;
            userProfile.CurrentDigitalPassword = userProfile.CurrentDigitalPassword ?? string.Empty;
            var updatePassword = userProfile.CurrentDigitalPassword != string.Empty;

            var userProfileRepository = _unitOfWork.GetRepository<UserProfileModel>();

            SqlParameter userIdParam = new SqlParameter("UserID", userProfile.UserID);
            SqlParameter updatePasswordParam = new SqlParameter("UpdatePassword", updatePassword);
            SqlParameter newPasswordParam = new SqlParameter("NewPassword", (userProfile.NewDigitalPassword == string.Empty) ? DBNull.Value : (object)userProfile.NewDigitalPassword);
            SqlParameter currentPasswordParam = new SqlParameter("CurrentPassword", userProfile.CurrentDigitalPassword);
            SqlParameter printSignatureParam = new SqlParameter("PrintSignature", userProfile.PrintSignature ?? string.Empty);
            SqlParameter modifiedOnParam = new SqlParameter("ModifiedOn", userProfile.ModifiedOn ?? DateTime.Now);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIdParam, updatePasswordParam, newPasswordParam, currentPasswordParam, printSignatureParam, modifiedOnParam };

            return _unitOfWork.EnsureInTransaction(userProfileRepository.ExecuteNQStoredProc, "usp_UpdateUserSignatureDetails", procParams,
                            forceRollback: userProfile.ForceRollback.GetValueOrDefault(false));
        }

        /// <summary>
        /// To Xml
        /// </summary>
        /// <param name="questions">Modal parameter</param>
        /// <returns></returns>
        private string QuestionsToXml(List<UserSecurityQuestionAnswerModel> questions)
        {
            var xEle = new XElement("RequestXMLValue",
                from question in questions
                select new XElement("Question",                        
                        new XElement("UserSecurityQuestionID", question.UserSecurityQuestionID),
                        new XElement("SecurityQuestionID",question.SecurityQuestionID),
                        new XElement("SecurityAnswer", question.SecurityAnswer),
                        new XElement("ModifiedOn", question.ModifiedOn ?? DateTime.Now)
                )
            );

            return xEle.ToString();
        }

        #endregion


    }
}
