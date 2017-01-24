using System.Collections.Generic;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ScreeningResultDataProvider : IScreeningResultDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ScreeningResultDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ScreeningResultModel> GetScreeningResults()
        {
            var repository = _unitOfWork.GetRepository<ScreeningResultModel>(SchemaName.ECI);
            var results = repository.ExecuteStoredProc("usp_GetScreeningResults");
            return results;
        }

        #endregion exposed functionality
    }
}