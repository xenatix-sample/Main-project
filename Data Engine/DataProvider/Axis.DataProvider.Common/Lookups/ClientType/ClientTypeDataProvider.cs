using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class ClientTypeDataProvider : IClientTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ClientTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ClientTypeModel> GetClientTypes()
        {
            var repository = _unitOfWork.GetRepository<ClientTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetClientTypeDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
