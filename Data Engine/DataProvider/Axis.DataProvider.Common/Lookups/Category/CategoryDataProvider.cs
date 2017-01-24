using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    /// <summary>
    /// Category Data Provider
    /// </summary>
    public class CategoryDataProvider : ICategoryDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CategoryDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CategoryDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the categories.
        /// </summary>
        /// <returns></returns>
        public Response<FinanceCategoryModel> GetCategories()
        {
            var repository = _unitOfWork.GetRepository<FinanceCategoryModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetCategoryDetails");

            return results;
        }

        /// <summary>
        /// Gets the type of the categories.
        /// </summary>
        /// <returns></returns>
        public Response<FinanceCategoryTypeModel> GetCategoriesType()
        {
            var repository = _unitOfWork.GetRepository<FinanceCategoryTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetCategoryTypeDetails");

            return results;
        }

        #endregion exposed functionality
    }
}