using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class ServiceRecordingSourceDataProvider : IServiceRecordingSourceDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingSourceDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public ServiceRecordingSourceDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the service recording source.
        /// </summary>
        /// <returns></returns>
        public Response<ServiceRecordingSourceModel> GetServiceRecordingSource()
        {
            var repository = _unitOfWork.GetRepository<ServiceRecordingSourceModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetServiceRecordingSource");
        }
    }
}
