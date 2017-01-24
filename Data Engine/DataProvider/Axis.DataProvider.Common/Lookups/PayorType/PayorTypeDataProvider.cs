using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
   
    public class PayorTypeDataProvider : IPayorTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public PayorTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<PayorTypeModel> GetPayorType()
        {
            var repository = _unitOfWork.GetRepository<PayorTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetPayorType");

            return results;
        }

        #endregion exposed functionality

    }

}
