using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class RecipientCodeDataProvider : IRecipientCodeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="RecipientCodeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public RecipientCodeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Recipient Codes.
        /// </summary>
        /// <returns></returns>
        public Response<RecipientCodeModel> GetRecipientCodes()
        {
            var repository = _unitOfWork.GetRepository<RecipientCodeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetRecipientCodeDetails");

            return results;
        }

        /// <summary>
        /// Gets the Recipient Code Modules.
        /// </summary>
        /// <returns></returns>
        public Response<RecipientCodeModel> GetRecipientCodeModuleComponents()
        {
            var repository = _unitOfWork.GetRepository<RecipientCodeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetRecipientCodeModuleComponentDetails");

            return results;
        }

        #endregion exposed functionality
    }
}
