
using Axis.Data.Repository;
using Axis.Data.Repository.Schema;
using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    public class TrackingFieldDataProvider : ITrackingFieldDataProvider
    {
        /// <summary>
        /// The _unit of work
        /// </summary>
        readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Initializes a new instance of the <see cref="TrackingFieldDataProvider"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>
        public TrackingFieldDataProvider(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Gets the tracking fields.
        /// </summary>
        /// <returns></returns>
        public Response<TrackingFieldModel> GetTrackingFields()
        {
            var repository = _unitOfWork.GetRepository<TrackingFieldModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetTrackingFieldDetails");
        }

        /// <summary>
        /// Gets the configured tracking fields.
        /// </summary>
        /// <returns></returns>
        public Response<TrackingFieldModel> GetTrackingFieldsConfigured()
        {
            var repository = _unitOfWork.GetRepository<TrackingFieldModel>(SchemaName.Reference);
            return repository.ExecuteStoredProc("usp_GetTrackingFieldModuleComponentDetails");
        }
    }
}
