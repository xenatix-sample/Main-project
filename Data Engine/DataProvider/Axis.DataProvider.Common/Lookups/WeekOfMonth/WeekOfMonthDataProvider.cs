using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class WeekOfMonthDataProvider : IWeekOfMonthDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public WeekOfMonthDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<WeekOfMonthModel> GetWeekOfMonth()
        {
            var repository = _unitOfWork.GetRepository<WeekOfMonthModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetWeekOfMonth");

            return results;
        }

        #endregion exposed functionality

        
    }
}
