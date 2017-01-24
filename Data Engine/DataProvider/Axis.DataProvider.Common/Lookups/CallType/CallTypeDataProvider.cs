using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class CallTypeDataProvider : ICallTypeDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallTypeDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public CallTypeDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the call status.
        /// </summary>
        /// <returns></returns>
        public Response<CallTypeModel> GetCallType()
        {
            var repository = _unitOfWork.GetRepository<CallTypeModel>(SchemaName.CallCenter);
            return repository.ExecuteStoredProc("usp_GetCallType");
        }
    }
}
