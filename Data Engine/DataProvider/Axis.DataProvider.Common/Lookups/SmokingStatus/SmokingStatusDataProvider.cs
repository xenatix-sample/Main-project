using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SmokingStatusDataProvider : ISmokingStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public SmokingStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<SmokingStatusModel> GetSmokingStatuses()
        {
            var repository = _unitOfWork.GetRepository<SmokingStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetSmokingStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
