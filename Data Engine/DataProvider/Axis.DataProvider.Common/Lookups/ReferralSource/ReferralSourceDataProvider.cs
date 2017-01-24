using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ReferralSourceDataProvider : IReferralSourceDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ReferralSourceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ReferralSourceModel> GetReferralSources()
        {
            var repository = _unitOfWork.GetRepository<ReferralSourceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetReferralSourceDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
