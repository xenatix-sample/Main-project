using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Service ConfigType DataProvider
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IServiceConfigTypeDataProvider" />
    public class ServiceConfigTypeDataProvider : IServiceConfigTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceConfigTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceConfigTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the service configuration types.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceConfigTypeModel> GetServiceConfigTypes()
        {
            var repository = _unitOfWork.GetRepository<ServiceConfigTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServiceConfigServiceType");
            return results;
        }

        #endregion exposed functionality

    }

}
