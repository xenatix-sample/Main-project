using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;
namespace Axis.DataProvider.Common
{
    public class SignatureStatusDataProvider : ISignatureStatusDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public SignatureStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the type of the service.
        /// </summary>
        /// <returns></returns>
        public Response<SignatureStatusModel> GetSignatureStatus()
        {
            var repository = _unitOfWork.GetRepository<SignatureStatusModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetSignatureStatus");
        }

        #endregion exposed functionality
        
    }

}
