using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.CallCenter.Repository.ServiceRecording
{
    /// <summary>
    /// 
    /// </summary>
    public interface IServiceRecordingRepository
    {

        /// <summary>
        /// Gets the service recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        Response<ServiceRecordingViewModel> GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID);

        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Response<ServiceRecordingViewModel> AddServiceRecording(ServiceRecordingViewModel model);

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Response<ServiceRecordingViewModel> UpdateServiceRecording(ServiceRecordingViewModel model);

        /// <summary>
        /// Gets the service recordings.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        Response<ServiceRecordingViewModel> GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate, DateTime? endDate);

        Response<ProgramUnitModel> GetProgramUnits(string datakey);
    }
}
