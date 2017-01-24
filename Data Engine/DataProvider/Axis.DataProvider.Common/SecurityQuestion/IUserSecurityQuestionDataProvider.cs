using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Common.User;

namespace Axis.DataProvider.Common.SecurityQuestion
{
    public interface IUserSecurityQuestionDataProvider
    {
        Response<UserSecurityQuestionModel> GetUserSecurityQuestions(int userID);
        Response<UserSecurityQuestionModel> SaveUserSecurityQuestions(int userID, List<UserSecurityQuestionModel> securityQuestions);
    }
}
