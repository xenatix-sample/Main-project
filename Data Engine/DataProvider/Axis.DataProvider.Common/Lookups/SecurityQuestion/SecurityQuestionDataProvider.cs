using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
using Axis.Model.Common.Lookups.SecurityQuestion;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityQuestionDataProvider : ISecurityQuestionDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SecurityQuestionDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SecurityQuestionDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the security questions.
        /// </summary>
        /// <returns></returns>
        public Response<SecurityQuestionModel> GetSecurityQuestions()
        {
            var repository = _unitOfWork.GetRepository<SecurityQuestionModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetSecurityQuestion");

            return results;
        }

        #endregion exposed functionality
    }
}