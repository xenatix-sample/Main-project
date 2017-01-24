using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class VoidRecordedServiceReasonDataProvider : IVoidRecordedServiceReasonDataProvider
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
        public VoidRecordedServiceReasonDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        #endregion initializations

        #region exposed functionality

        /// <summary>
        /// Gets the ServiceRecordingVoidReasonDetails of the service.
        /// </summary>
        /// <returns></returns>
        public Response<VoidRecordedServiceReasonModel> GetVoidServiceRecordingReasonDetails()
        {
            var repository = _unitOfWork.GetRepository<VoidRecordedServiceReasonModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetServiceRecordingVoidReasonDetails");
        }

        #endregion exposed functionality
        
    }
}
