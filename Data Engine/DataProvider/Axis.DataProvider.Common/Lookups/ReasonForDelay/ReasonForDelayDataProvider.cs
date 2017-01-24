using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ReasonForDelayDataProvider : IReasonForDelayDataProvider
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ReasonForDelayDataProvider(IUnitOfWork unitOfWork)
        {
            this._unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets Reason for Delay
        /// </summary>
        /// <returns></returns>
        public Response<ReasonForDelayModel> GetReasonForDelay()
        {
            var repository = _unitOfWork.GetRepository<ReasonForDelayModel>(SchemaName.ECI);
            var results = repository.ExecuteStoredProc("usp_GetReasonForDelay");
            return results;
        }
    }
}