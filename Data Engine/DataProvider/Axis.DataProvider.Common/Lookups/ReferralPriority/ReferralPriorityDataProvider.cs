using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ReferralPriorityDataProvider : IReferralPriorityDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ReferralPriorityDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ReferralPriorityModel> GetReferralPriorities()
        {
            var repository = _unitOfWork.GetRepository<ReferralPriorityModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralPriorities");

            return results;
        }

        #endregion exposed functionality
    }
}
