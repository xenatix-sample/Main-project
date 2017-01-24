using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ScheduleTypeDataProvider : IScheduleTypeDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ScheduleTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ScheduleTypeModel> GetScheduleType()
        {
 	        var repository = _unitOfWork.GetRepository<ScheduleTypeModel>(SchemaName.Scheduling);
            var results = repository.ExecuteStoredProc("usp_GetScheduleTypes");

            return results;
        }

        #endregion exposed functionality        
    
    
    }
}
