using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralStatusDataProvider : IReferralStatusDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the referral statuses.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralStatusModel> GetReferralStatuses()
        {
            var repository = _unitOfWork.GetRepository<ReferralStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralStatuses");

            return results;
        }

        #endregion exposed functionality
    }
}