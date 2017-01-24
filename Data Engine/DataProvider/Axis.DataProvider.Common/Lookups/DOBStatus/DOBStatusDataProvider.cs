using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class DOBStatusDataProvider : IDOBStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public DOBStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<DOBStatusModel> GetDOBStatuses()
        {
            var repository = _unitOfWork.GetRepository<DOBStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetDOBStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
