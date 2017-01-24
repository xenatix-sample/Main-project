using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class DayOfWeekDataProvider : IDayOfWeekDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public DayOfWeekDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<DayOfWeekModel> GetDayOfWeek()
        {
            var repository = _unitOfWork.GetRepository<DayOfWeekModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetDaysOfWeek");

            return results;
        }

        #endregion exposed functionality

        
    }
}
