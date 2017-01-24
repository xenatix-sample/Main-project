using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ReceiveCorrespondenceDataProvider : IReceiveCorrespondenceDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReceiveCorrespondenceDataProvider(IUnitOfWork unitOfWork)
        {
            this.unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the receive correspondence.
        /// </summary>
        /// <returns></returns>
        public Response<ReceiveCorrespondenceModel> GetReceiveCorrespondence()
        {
            var repository = unitOfWork.GetRepository<ReceiveCorrespondenceModel>(SchemaName.Reference);
            var results = repository.ExecuteStoredProc("usp_ReceiveCorrespondenceDetails");
            return results;
        }
    }
}