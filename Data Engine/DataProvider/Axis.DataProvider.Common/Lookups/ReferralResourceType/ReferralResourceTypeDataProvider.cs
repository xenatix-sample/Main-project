using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IReferralResourceTypeDataProvider" />
    public class ReferralResourceTypeDataProvider : IReferralResourceTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralResourceTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralResourceTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the referral resource.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralResourceTypeModel> GetReferralResourceType()
        {
            var repository = _unitOfWork.GetRepository<ReferralResourceTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralResourceTypes");

            return results;
        }

        #endregion exposed functionality

    }

}
