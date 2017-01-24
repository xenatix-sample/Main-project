using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class AdmissionReasonDataProvider : IAdmissionReasonDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public AdmissionReasonDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<AdmissionReasonModel> GetAdmissionReasonTypes()
        {
            var repository = _unitOfWork.GetRepository<AdmissionReasonModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetAdmissionReasonDetails");
            return results;
        }

        #endregion exposed functionality
    }
}