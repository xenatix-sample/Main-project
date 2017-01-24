using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Service Workflow Type DataProvider
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IServiceWorkflowTypeDataProvider" />
    public class ServiceWorkflowTypeDataProvider : IServiceWorkflowTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceWorkflowTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceWorkflowTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the service workflow types.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceWorkflowTypeModel> GetServiceWorkflowTypes()
        {
            var repository = _unitOfWork.GetRepository<ServiceWorkflowTypeModel>(SchemaName.Core);
            var results = repository.ExecuteStoredProc("usp_GetModuleComponentsForServiceMapping");
            return results;
        }

        #endregion exposed functionality

    }

}
