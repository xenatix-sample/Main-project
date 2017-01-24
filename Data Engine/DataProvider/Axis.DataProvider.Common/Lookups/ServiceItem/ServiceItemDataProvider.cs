using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceItemDataProvider : IServiceItemDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceItemDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceItemDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the Service Items.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceItemModel> GetServiceItems()
        {
            var repository = _unitOfWork.GetRepository<ServiceItemModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetServices");

            return results;
        }

        #endregion exposed functionality
    }
}
