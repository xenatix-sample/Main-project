using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ReferralDispositionOutcomeTypeDataProvider : IReferralDispositionOutcomeTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralDispositionOutcomeTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the referral disposition outcome.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralDispositionOutcomeTypeModel> GetReferralDispositionOutcomeType()
        {
            var repository = _unitOfWork.GetRepository<ReferralDispositionOutcomeTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralDispositionOutcomes");

            return results;
        }

        #endregion exposed functionality
    }
}
