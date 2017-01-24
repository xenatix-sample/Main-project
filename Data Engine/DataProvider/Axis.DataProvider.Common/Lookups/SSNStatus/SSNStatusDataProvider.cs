using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class SSNStatusDataProvider : ISSNStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public SSNStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<SSNStatusModel> GetSSNStatuses()
        {
            var repository = _unitOfWork.GetRepository<SSNStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetSSNStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
