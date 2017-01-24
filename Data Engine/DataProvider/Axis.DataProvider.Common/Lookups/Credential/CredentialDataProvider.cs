using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class CredentialDataProvider : ICredentialDataProvider
    {
        #region initializations

        readonly IUnitOfWork _unitOfWork;

        public CredentialDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        public Response<CredentialModel> GetCredentials()
        {
            var repository = _unitOfWork.GetRepository<CredentialModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetCredentials");

            return results;
        }

        #endregion exposed functionality

    }
}
