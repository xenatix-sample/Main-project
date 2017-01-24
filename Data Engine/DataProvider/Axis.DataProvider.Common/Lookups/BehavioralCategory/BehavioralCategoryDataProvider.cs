using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class BehavioralCategoryDataProvider : IBehavioralCategoryDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="BehavioralCategoryDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public BehavioralCategoryDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the behavioral categories.
        /// </summary>
        /// <returns></returns>
        public Response<BehavioralCategoryModel> GetBehavioralCategories()
        {
            var repository = _unitOfWork.GetRepository<BehavioralCategoryModel>(SchemaName.CallCenter);
            return repository.ExecuteStoredProc("usp_GetBehavioralCategory");
        }
    }
}
