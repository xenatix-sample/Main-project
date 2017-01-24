using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    public class PayorExpirationReasonDataProvider : IPayorExpirationReasonDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PayorExpirationReasonDataProvider" /> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PayorExpirationReasonDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the payor expiration reason.
        /// </summary>
        /// <returns></returns>
        public Response<PayorExpirationReasonModel> GetPayorExpirationReason()
        {
            var repository = _unitOfWork.GetRepository<PayorExpirationReasonModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetPayorExpirationReasons");

            return results;
        }

        #endregion exposed functionality

    }

}
