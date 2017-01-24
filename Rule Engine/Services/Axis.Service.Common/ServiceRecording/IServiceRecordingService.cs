using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;

namespace Axis.Service.Common.ServiceRecording
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceRecordingService
    {
        /// <summary>
        /// Gets the Service Recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        Response<ServiceRecordingModel> GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID);

        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Response<ServiceRecordingModel> AddServiceRecording(ServiceRecordingModel model);

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Response<ServiceRecordingModel> UpdateServiceRecording(ServiceRecordingModel model);

        /// <summary>
        /// Gets the service recordings.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        Response<ServiceRecordingModel> GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate, DateTime? endDate);

        Response<ProgramUnitModel> GetProgramUnits(string datakey);
    }
}
