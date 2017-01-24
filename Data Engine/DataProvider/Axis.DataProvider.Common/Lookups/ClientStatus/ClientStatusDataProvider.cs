using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ClientStatusDataProvider : IClientStatusDataProvider
    {
         /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ClientStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the client status.
        /// </summary>
        /// <returns></returns>
        public Response<ClientStatusModel> GetClientStatus()
        {
            var repository = _unitOfWork.GetRepository<ClientStatusModel>(SchemaName.CallCenter);
            return repository.ExecuteStoredProc("usp_GetClientStatus");
        }
    
    }
}
