using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class CallStatusDataProvider : ICallStatusDataProvider
    {
         /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallStatusDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CallStatusDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }


        /// <summary>
        /// Gets the call status.
        /// </summary>
        /// <returns></returns>
        public Response<CallStatusModel> GetCallStatus()
        {
            var repository = _unitOfWork.GetRepository<CallStatusModel>(SchemaName.CallCenter);
            var results = repository.ExecuteStoredProc("usp_GetCallStatusDetails");

            return results;
        }
    }
}
