using System.Collections.Generic;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceDurationDataProvider : IServiceDurationDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ServiceDurationDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ServiceDurationModel> GetServiceDurations()
        {
            var repository = _unitOfWork.GetRepository<ServiceDurationModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServiceDuration");
            return results;
        }

        #endregion exposed functionality
    }
}