using Axis.Model.Common;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;

namespace Axis.DataProvider.Common
{
    public class CollateralTypeDataProvider : ICollateralTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public CollateralTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<CollateralTypeModel> GetCollateralType()
        {
            var repository = _unitOfWork.GetRepository<CollateralTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetCollateralType");

            return results;
        }
        #endregion exposed functionality
    }
}
