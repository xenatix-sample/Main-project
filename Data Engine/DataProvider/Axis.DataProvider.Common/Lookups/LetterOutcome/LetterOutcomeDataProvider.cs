using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class LetterOutcomeDataProvider : ILetterOutcomeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="LetterOutcomeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public LetterOutcomeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the letter outcome.
        /// </summary>
        /// <returns>Response&lt;LetterOutcomeModel&gt;.</returns>
        public Response<LetterOutcomeModel> GetLetterOutcome()
        {
            var repository = _unitOfWork.GetRepository<LetterOutcomeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetLetterOutcomeOptions"); 

            return results;
        }
        #endregion exposed functionality
    }
}
