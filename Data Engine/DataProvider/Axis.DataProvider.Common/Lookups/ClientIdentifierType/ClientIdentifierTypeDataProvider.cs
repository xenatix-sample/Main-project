using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ClientIdentifierTypeDataProvider : IClientIdentifierTypeDataProvider
    {
        #region initializations

        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ClientIdentifierTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ClientIdentifierTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the client identifier types.
        /// </summary>
        /// <returns></returns>
        public Response<ClientIdentifierTypeModel> GetClientIdentifierTypes()
        {
            var repository = _unitOfWork.GetRepository<ClientIdentifierTypeModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_GetClientIdentifierTypeDetails");

            return results;
        }

        #endregion exposed functionality
    }
}