using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataProvider.Common.IResourceTypeDataProvider" />
    public class ResourceTypeDataProvider : IResourceTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ResourceTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the resource.
        /// </summary>
        /// <returns></returns>
        public Response<ResourceTypeModel> GetResourceTypeDetails()
        {
            var repository = _unitOfWork.GetRepository<ResourceTypeModel>(SchemaName.Scheduling);
            var results = repository.ExecuteStoredProc("usp_GetResourceTypeDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
