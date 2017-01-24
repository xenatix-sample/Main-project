using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class MaritalStatusDataProvider : IMaritalStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public MaritalStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<MaritalStatusModel> GetMaritalStatuses()
        {
            var repository = _unitOfWork.GetRepository<MaritalStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetMaritalStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
