using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    // ReSharper disable once ClassNeverInstantiated.Global
    public class VeteranStatusDataProvider : IVeteranStatusDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public VeteranStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<VeteranStatusModel> GetVeteranStatuses()
        {
            var repository = _unitOfWork.GetRepository<VeteranStatusModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetVeteranStatusDetails");

            return results;
        }

        #endregion exposed functionality

    }

}
