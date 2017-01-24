using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class PrefixTypeDataProvider : IPrefixTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrefixTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public PrefixTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the prefix.
        /// </summary>
        /// <returns></returns>
        public Response<TitleModel> GetPrefixType()
        {
            var repository = _unitOfWork.GetRepository<TitleModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetTitleDetails");

            return results;
        }

        #endregion exposed functionality
    }
}