using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Common;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class ServiceRecordingController : BaseApiController
    {
        /// <summary>
        /// The service recording rule engine
        /// </summary>
        private readonly IServiceRecordingDataProvider _serviceRecordingDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingController" /> class.
        /// </summary>
        /// <param name="serviceRecordingDataProvider">The service recording data provider.</param>
        public ServiceRecordingController(IServiceRecordingDataProvider serviceRecordingDataProvider)
        {
            _serviceRecordingDataProvider = serviceRecordingDataProvider;
        }

        /// <summary>
        /// Gets the service recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingDataProvider.GetServiceRecording(SourceHeaderID, ServiceRecordingSourceID), Request);
        }

        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddServiceRecording(ServiceRecordingModel model)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingDataProvider.AddServiceRecording(model), Request);
        }

        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateServiceRecording(ServiceRecordingModel model)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingDataProvider.UpdateServiceRecording(model), Request);
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
        public IHttpActionResult GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate = null, DateTime? endDate = null)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingDataProvider.GetServiceRecordings(ServiceRecordingSourceID, ContactID, startDate, endDate), Request);
        }


        [HttpGet]
        public IHttpActionResult GetProgramUnits(string datakey)
        {
            return new HttpResult<Response<ProgramUnitModel>>(_serviceRecordingDataProvider.GetProgramUnits(datakey), Request);
        }
    }
}