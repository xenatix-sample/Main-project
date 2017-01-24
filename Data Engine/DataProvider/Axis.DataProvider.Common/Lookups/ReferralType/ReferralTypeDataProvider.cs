using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IReferralTypeDataProvider" />
    public class ReferralTypeDataProvider : IReferralTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the referral.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralTypeModel> GetReferralType()
        {
            var repository = _unitOfWork.GetRepository<ReferralTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralTypes");

            return results;
        }

        #endregion exposed functionality

    }

}
