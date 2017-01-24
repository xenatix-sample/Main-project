using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.ReferralOrganizationDataProvider" />
    public class ReferralOrganizationDataProvider : IReferralOrganizationDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralOrganizationDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralOrganizationDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the referral Organizations.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralOrganizationModel> GetOrganizations()
        {
            var repository = _unitOfWork.GetRepository<ReferralOrganizationModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralOrganization");

            return results;
        }

        #endregion exposed functionality

    }

}
