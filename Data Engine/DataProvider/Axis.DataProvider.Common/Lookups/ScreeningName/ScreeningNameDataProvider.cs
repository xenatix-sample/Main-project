using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ScreeningNameDataProvider : IScreeningNameDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ScreeningNameDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ScreeningNameModel> GetScreeningName()
        {
            var repository = _unitOfWork.GetRepository<ScreeningNameModel>(SchemaName.ECI);
            var results = repository.ExecuteStoredProc("usp_GetScreeningName");
            return results;
        }

        #endregion exposed functionality
    }
}