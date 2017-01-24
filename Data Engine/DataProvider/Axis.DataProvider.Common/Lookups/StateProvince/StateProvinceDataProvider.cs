using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class StateProvinceDataProvider : IStateProvinceDataProvider
    {
         #region initializations

        readonly IUnitOfWork _unitOfWork;

        public StateProvinceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<StateProvinceModel> GetStateProvince()
        {
            var repository = _unitOfWork.GetRepository<StateProvinceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetStateDetails");

            return results;
        }

        #endregion exposed functionality

       
    }
}
