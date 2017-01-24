using System;
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class CauseOfDeathDataProvider : ICauseOfDeathDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public CauseOfDeathDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality
        public Response<CauseOfDeathModel> GetCauseOfDeath()
        {
            var repository = _unitOfWork.GetRepository<CauseOfDeathModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetCauseOfDeath");

            return results;
        }

        #endregion exposed functionality
    }
}
