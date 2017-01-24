using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ConversionStatusDataProvider : IConversionStatusDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConversionStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ConversionStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Conversion Statuses.
        /// </summary>
        /// <returns></returns>
        public Response<ConversionStatusModel> GetConversionStatuses()
        {
            var repository = _unitOfWork.GetRepository<ConversionStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetConversionStatusDetails");

            return results;
        }

        #endregion exposed functionality
    }
}
