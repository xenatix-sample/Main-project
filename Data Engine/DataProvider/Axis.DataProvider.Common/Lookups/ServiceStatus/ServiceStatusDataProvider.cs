using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceStatusDataProvider : IServiceStatusDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Service Statuses.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceStatusModel> GetServiceStatuses()
        {
            var repository = _unitOfWork.GetRepository<ServiceStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServiceStatusDetails");

            return results;
        }

        /// <summary>
        /// Gets the service to service statuses.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceStatusModel> GetServiceToServiceStatuses()
        {
            var repository = _unitOfWork.GetRepository<ServiceStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServicestoServiceStatusMapping");

            return results;
        }

        /// <summary>
        /// Gets the Configured Service Statuses.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceStatusModel> GetServiceStatusesConfigured()
        {
            var repository = _unitOfWork.GetRepository<ServiceStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServiceStatusModuleComponentDetails");

            return results;
        }

        #endregion exposed functionality
    }
}