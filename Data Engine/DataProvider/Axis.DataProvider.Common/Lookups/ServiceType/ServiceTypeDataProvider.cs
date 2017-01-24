using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceTypeDataProvider : IServiceTypeDataProvider
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
        public ServiceTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceTypeModel> GetServiceType()
        {
            var repository = _unitOfWork.GetRepository<ServiceTypeModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetServiceTypeDetails");
        }

        #endregion exposed functionality
        
    }
}
