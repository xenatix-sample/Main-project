using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class PayorDataProvider : IPayorDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayorDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PayorDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the payors.
        /// </summary>
        /// <returns></returns>
        public Response<PayorModel> GetPayors()
        {
            var repository = _unitOfWork.GetRepository<PayorModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetPayorDetails");

            return results;
        }

        #endregion exposed functionality
    }
}