using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class CountyDataProvider : ICountyDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public CountyDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<CountyModel> GetCounty()
        {
            var repository = _unitOfWork.GetRepository<CountyModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetCountyDetails");

            return results;
        }

        public Response<CountyModel> GetOrganizationCounty()
        {
            var repository = _unitOfWork.GetRepository<CountyModel>(SchemaName.Core);
            var results = repository.ExecuteStoredProc("usp_GetOrganizationCounty");

            return results;
        }

        #endregion exposed functionality

    }
}
