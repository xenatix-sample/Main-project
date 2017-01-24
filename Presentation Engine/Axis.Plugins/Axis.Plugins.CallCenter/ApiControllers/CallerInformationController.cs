using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Repository.CallerInformation;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Web.Http;

namespace Axis.Plugins.CallCenter.ApiControllers
{
    public class CallerInformationController : BaseApiController
    {
        /// <summary>
        /// The caller information repository
        /// </summary>
        private readonly ICallerInformationRepository _callerInformationRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationController"/> class.
        /// </summary>
        /// <param name="callerInformationRepository">The call center summary repository.</param>
        public CallerInformationController(ICallerInformationRepository callerInformationRepository)
        {
            _callerInformationRepository = callerInformationRepository;
        }

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="CallCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<CallCenterProgressNoteViewModel> GetCallerInformation(long CallCenterHeaderID)
        {
            return _callerInformationRepository.GetCallerInformation(CallCenterHeaderID);
        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<CallCenterProgressNoteViewModel> AddCallerInformation(CallCenterProgressNoteViewModel model)
        {
            return _callerInformationRepository.AddCallerInformation(model);
        }

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<CallCenterProgressNoteViewModel> UpdateCallerInformation(CallCenterProgressNoteViewModel model)
        {
            return _callerInformationRepository.UpdateCallerInformation(model);
        }


        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name = "callCenterHeaderID" > The call center header identifier.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<CallCenterProgressNoteViewModel> UpdateModifiedOn(CallCenterProgressNoteViewModel model)
        {
            return _callerInformationRepository.UpdateModifiedOn(model);
        }


        [HttpGet]
        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponse(long CallCenterHeaderID,long AssessmentID)
        {
            return _callerInformationRepository.GetCallCenterAssessmentResponse(CallCenterHeaderID, AssessmentID);
        }

        [HttpGet]
        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponseByContactID(long contactID)
        {
            return _callerInformationRepository.GetCallCenterAssessmentResponseByContactID(contactID);
        }

        [HttpPost]
        public Response<CallCenterAssessmentResponseModel> AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel model)
        {
            return _callerInformationRepository.AddCallCenterAssessmentResponse(model);
        }
    }
}