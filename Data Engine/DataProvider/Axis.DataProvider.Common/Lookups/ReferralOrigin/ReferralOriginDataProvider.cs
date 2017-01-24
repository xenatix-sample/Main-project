using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ReferralOriginDataProvider : IReferralOriginDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ReferralOriginDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ReferralOriginModel> GetReferralOrigin()
        {
            var repository = _unitOfWork.GetRepository<ReferralOriginModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralOrigin");

            return results;
        }

        #endregion exposed functionality

    }

}
