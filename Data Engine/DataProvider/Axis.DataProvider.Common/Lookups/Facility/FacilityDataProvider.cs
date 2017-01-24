using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class FacilityDataProvider : IFacilityDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public FacilityDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<FacilityModel> GetFacility()
        {
            var repository = _unitOfWork.GetRepository<FacilityModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetFacility");

            return results;
        }

        #endregion exposed functionality
    }
}
