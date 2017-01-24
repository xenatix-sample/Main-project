using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.CallCenter.CallerInformation;
using Axis.Model;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.CallCenter
{
    public class CallerInformationController : BaseApiController
    {
        /// <summary>
        /// The _caller information data provider
        /// </summary>
        private readonly ICallerInformationDataProvider _callerInformationDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationController"/> class.
        /// </summary>
        /// <param name="callerInformationDataProvider">The caller information data provider.</param>
        public CallerInformationController(ICallerInformationDataProvider callerInformationDataProvider)
        {
            _callerInformationDataProvider = callerInformationDataProvider;
        }

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCallerInformation(long callCenterHeaderID)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationDataProvider.GetCallerInformation(callCenterHeaderID), Request);
        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddCallerInformation(CallCenterProgressNoteModel model)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationDataProvider.AddCallerInformation(model), Request);
        }

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateCallerInformation(CallCenterProgressNoteModel model)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationDataProvider.UpdateCallerInformation(model), Request);
        }

        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateModifiedOn(CallCenterProgressNoteModel model)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationDataProvider.UpdateModifiedOn(model), Request);
        }

        [HttpGet]
        public IHttpActionResult GetCallCenterAssessmentResponse(long callCenterHeaderID, long AssessmentID)
        {
            return new HttpResult<Response<CallCenterAssessmentResponseModel>>(_callerInformationDataProvider.GetCallCenterAssessmentResponse(callCenterHeaderID, AssessmentID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetCallCenterAssessmentResponseByContactID(long contactID)
        {
            return new HttpResult<Response<CallCenterAssessmentResponseModel>>(_callerInformationDataProvider.GetCallCenterAssessmentResponseByContactID(contactID), Request);
        }

        [HttpPost]
        public IHttpActionResult AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel model)
        {
            return new HttpResult<Response<CallCenterAssessmentResponseModel>>(_callerInformationDataProvider.AddCallCenterAssessmentResponse(model), Request);
        }
    }
}