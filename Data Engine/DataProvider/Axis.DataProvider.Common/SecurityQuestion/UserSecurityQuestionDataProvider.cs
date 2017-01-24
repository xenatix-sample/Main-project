using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Xml.Linq;
using System.Linq;
using Axis.Data.Repository;
using Axis.Model.Common;
using Axis.Model.Common.User;

namespace Axis.DataProvider.Common.SecurityQuestion
{
    public class UserSecurityQuestionDataProvider : IUserSecurityQuestionDataProvider
    {
        #region Class Variables

        private readonly IUnitOfWork _unitOfWork;

        #endregion Class Variables

        #region Constructors

        public UserSecurityQuestionDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion Constructors

        #region Public Methods

        public Response<UserSecurityQuestionModel> GetUserSecurityQuestions(int userID)
        {
            var userProfileRepository = _unitOfWork.GetRepository<UserSecurityQuestionModel>();

            SqlParameter userIDParam = new SqlParameter("UserID", userID);
            List<SqlParameter> procParams = new List<SqlParameter>() { userIDParam };
            var result = userProfileRepository.ExecuteStoredProc("usp_GetUserSecurityQuestions", procParams);

            return result;
        }

        public Response<UserSecurityQuestionModel> SaveUserSecurityQuestions(int userID, List<UserSecurityQuestionModel> securityQuestions)
        {
            var userSecurityQuestionRepository = _unitOfWork.GetRepository<UserSecurityQuestionModel>();
            var userSecurityQuestionResult = new Response<UserSecurityQuestionModel>() { ResultCode = 0 };

            var questionsToAdd = securityQuestions.Where(questions => questions.UserSecurityQuestionID == 0).ToList();
            var questionsToUpdate = securityQuestions.Where(questions => questions.UserSecurityQuestionID != 0).ToList();

            if (questionsToAdd.Count > 0)
            {
                SqlParameter userIDParam = new SqlParameter("UserID", userID);
                var requestXmlValueParam = new SqlParameter("QuestionXml", QuestionsToXml(questionsToAdd));
                var phoneParameters = new List<SqlParameter>() { userIDParam, requestXmlValueParam };
                userSecurityQuestionResult = _unitOfWork.EnsureInTransaction<Response<UserSecurityQuestionModel>>(userSecurityQuestionRepository.ExecuteNQStoredProc, "usp_AddUserSecurityQuestions", phoneParameters, forceRollback: questionsToAdd.Any(x => x.ForceRollback.GetValueOrDefault(false)));

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
                userSecurityQuestionResult = _unitOfWork.EnsureInTransaction<Response<UserSecurityQuestionModel>>(userSecurityQuestionRepository.ExecuteNQStoredProc, "usp_UpdateUserSecurityQuestions", securityQuestionParameters, forceRollback: questionsToAdd.Any(x => x.ForceRollback.GetValueOrDefault(false)));
            }

            return userSecurityQuestionResult;
        }

        private string QuestionsToXml(List<UserSecurityQuestionModel> questions)
        {
            var xEle = new XElement("RequestXMLValue",
                from question in questions
                select new XElement("Question",
                        new XElement("SecurityQuestionID", question.SecurityQuestionID),
                        new XElement("UserSecurityQuestionID", question.UserSecurityQuestionID),
                        new XElement("SecurityAnswer", question.SecurityAnswer),
                        new XElement("ModifiedOn", question.ModifiedOn ?? DateTime.Now)
                )
            );

            return xEle.ToString();
        }

        #endregion
    }
}
