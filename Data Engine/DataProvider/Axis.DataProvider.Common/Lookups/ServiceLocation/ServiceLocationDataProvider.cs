using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceLocationDataProvider : IServiceLocationDataProvider
    {

        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ServiceLocationDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ServiceLocationModel> GetServiceLocation()
        {
            var repository = _unitOfWork.GetRepository<ServiceLocationModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServiceLocationDetails");
            return results;
        }

        public Response<ServiceLocationModel> GetServiceLocationModuleComponents()
        {
            var repository = _unitOfWork.GetRepository<ServiceLocationModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServiceLocationModuleComponentDetails");
            return results;
        }

        #endregion exposed functionality

    }
}
