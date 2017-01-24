using Axis.Model.Common;
using Axis.Plugins.CallCenter.Repository.ServiceRecording;
using Axis.PresentationEngine.Helpers.Controllers;
using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Web.Mvc;

/// <summary>
/// 
/// </summary>
namespace Axis.Plugins.CallCenter.ApiControllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Controllers.BaseApiController" />
    public class ServiceRecordingController : BaseApiController
    {
        /// <summary>
        /// The Service Recording repository
        /// </summary>
        private readonly IServiceRecordingRepository _serviceRecordingRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingController" /> class.
        /// </summary>
        /// <param name="serviceRecordingRepository">The service recording repository.</param>
        public ServiceRecordingController(IServiceRecordingRepository serviceRecordingRepository)
        {
            _serviceRecordingRepository = serviceRecordingRepository;
        }

        /// <summary>
        /// Gets the service recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ServiceRecordingViewModel> GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID)
        {
            return _serviceRecordingRepository.GetServiceRecording(SourceHeaderID, ServiceRecordingSourceID);
        }

        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ServiceRecordingViewModel> AddServiceRecording(ServiceRecordingViewModel model)
        {
            return _serviceRecordingRepository.AddServiceRecording(model);
        }

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ServiceRecordingViewModel> UpdateServiceRecording(ServiceRecordingViewModel model)
        {
            return _serviceRecordingRepository.UpdateServiceRecording(model);
        }

        /// <summary>
        /// Gets the service recordings.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ServiceRecordingViewModel> GetServiceRecordings(int ServiceRecordingSourceID, long ContactID,DateTime? startDate=null, DateTime? endDate=null)
        {
            return _serviceRecordingRepository.GetServiceRecordings(ServiceRecordingSourceID, ContactID,startDate, endDate);
        }

        [HttpGet]
        public Response<ProgramUnitModel> GetProgramUnits(string datakey)
        {
            return _serviceRecordingRepository.GetProgramUnits(datakey);
        }


    }
}
