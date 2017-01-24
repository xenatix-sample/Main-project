using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Service.Common.ServiceRecording;
using System;

namespace Axis.RuleEngine.Common.ServiceRecording
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Common.ServiceRecording.IServiceRecordingRuleEngine" />
    public class ServiceRecordingRuleEngine : IServiceRecordingRuleEngine
    {
        /// <summary>
        /// The service recording service
        /// </summary>
        private readonly IServiceRecordingService _serviceRecordingService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingRuleEngine" /> class.
        /// </summary>
        /// <param name="serviceRecordingService">The service recording service.</param>
        public ServiceRecordingRuleEngine(IServiceRecordingService serviceRecordingService)
        {
            _serviceRecordingService = serviceRecordingService;
        }

        /// <summary>
        /// Gets the Service Recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID)
        {
            return _serviceRecordingService.GetServiceRecording(SourceHeaderID, ServiceRecordingSourceID);
        }

        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> AddServiceRecording(ServiceRecordingModel model)
        {
            return _serviceRecordingService.AddServiceRecording(model);
        }

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<ServiceRecordingModel> UpdateServiceRecording(ServiceRecordingModel model)
        {
            return _serviceRecordingService.UpdateServiceRecording(model);
        }

        /// <summary>
        /// Gets the service recordings.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<ServiceRecordingModel> GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate, DateTime? endDate)
        {
            return _serviceRecordingService.GetServiceRecordings(ServiceRecordingSourceID, ContactID, startDate, endDate);
        }

        public Response<ProgramUnitModel> GetProgramUnits(string datakey)
        {
            return _serviceRecordingService.GetProgramUnits(datakey);
        }

    }
}
