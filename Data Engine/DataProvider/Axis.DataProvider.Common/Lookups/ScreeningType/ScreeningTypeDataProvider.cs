using System.Collections.Generic;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ScreeningTypeDataProvider : IScreeningTypeDataProvider
    {
        #region initializations

        private readonly IUnitOfWork _unitOfWork;

        public ScreeningTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ScreeningTypeModel> GetScreeningTypes()
        {
            var repository = _unitOfWork.GetRepository<ScreeningTypeModel>(SchemaName.ECI);
            var results = repository.ExecuteStoredProc("usp_GetScreeningType");
            return results;
        }

        #endregion exposed functionality
    }
}