using System.Collections.Generic;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceCoordinatorDataProvider : IServiceCoordinatorDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ServiceCoordinatorDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ServiceCoordinatorModel> GetServiceCoordinators()
        {
            var repository = _unitOfWork.GetRepository<ServiceCoordinatorModel>(SchemaName.ECI);
            var results = repository.ExecuteStoredProc("usp_GetSpecialist");
            return results;
        }

        #endregion exposed functionality
    }
}