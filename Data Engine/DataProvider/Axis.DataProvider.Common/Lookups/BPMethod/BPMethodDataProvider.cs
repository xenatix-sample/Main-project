using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class BPMethodDataProvider : IBPMethodDataProvider
    {
         #region initializations

        readonly IUnitOfWork _unitOfWork;

        public BPMethodDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<BPMethodModel> GetBPMethods()
        {
            var repository = _unitOfWork.GetRepository<BPMethodModel>(SchemaName.Clinical);
            var results = repository.ExecuteStoredProc("usp_GetBPMethod");

            return results;
        }

        #endregion exposed functionality
    }
}
