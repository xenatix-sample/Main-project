using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class MonthDataProvider : IMonthDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public MonthDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<MonthModel> GetMonth()
        {
            var repository = _unitOfWork.GetRepository<MonthModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetMonth");

            return results;
        }

        #endregion exposed functionality

        
    }
}
