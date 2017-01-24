using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ImmunizationStatusDataProvider : IImmunizationStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public ImmunizationStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<ImmunizationStatusModel> GetImmunizationStatuses()
        {
            var repository = _unitOfWork.GetRepository<ImmunizationStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetImmunizationStatuses");

            return results;
        }

        #endregion exposed functionality
    }
}
