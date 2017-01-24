using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IReferralCategoryDataProvider" />
    public class ReferralCategorySourceDataProvider : IReferralCategorySourceDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralCategoryDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReferralCategorySourceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the referral category.
        /// </summary>
        /// <returns></returns>
        public Response<ReferralCategorySourceModel> GetReferralCategorySource()
        {
            var repository = _unitOfWork.GetRepository<ReferralCategorySourceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralCategorySource");

            return results;
        }

        #endregion exposed functionality

    }

}
