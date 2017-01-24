using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class LegalStatusDataProvider : ILegalStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public LegalStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<LegalStatusModel> GetLegalStatuses()
        {
            var repository = _unitOfWork.GetRepository<LegalStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetLegalStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
