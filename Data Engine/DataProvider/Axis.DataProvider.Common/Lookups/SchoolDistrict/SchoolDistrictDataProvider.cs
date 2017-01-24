using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// School district data provider
    /// </summary>
    public class SchoolDistrictDataProvider : ISchoolDistrictDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="SchoolDistrictDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SchoolDistrictDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the school districts.
        /// </summary>
        /// <returns></returns>
        public Response<SchoolDistrictModel> GetSchoolDistricts()
        {
            var repository = _unitOfWork.GetRepository<SchoolDistrictModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetSchoolDistrictDetails");

            return results;
        }

        #endregion exposed functionality
    }
}