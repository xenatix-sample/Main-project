using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceDataProvider : IServiceDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the service location.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceModel> GetService()
        {
            var repository = _unitOfWork.GetRepository<ServiceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServices");
            return results;
        }

        /// <summary>
        /// Gets the service details.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceModel> GetServiceDetails()
        {
            var repository = _unitOfWork.GetRepository<ServiceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServiceDetails");
            return results;
        }

        #endregion exposed functionality
    }
}