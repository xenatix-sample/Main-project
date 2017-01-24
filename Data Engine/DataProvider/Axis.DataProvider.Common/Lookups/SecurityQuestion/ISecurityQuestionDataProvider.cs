using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public interface ISecurityQuestionDataProvider
    {
        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        Response<SecurityQuestionModel> GetSecurityQuestions();
    }
}