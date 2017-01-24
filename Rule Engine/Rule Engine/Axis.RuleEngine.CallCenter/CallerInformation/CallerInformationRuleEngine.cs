using Axis.Model;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Service.CallCenter.CallerInformation;
using System;

namespace Axis.RuleEngine.CallCenter.CallerInformation
{
    public class CallerInformationRuleEngine : ICallerInformationRuleEngine
    {
        private readonly ICallerInformationService _callerInformationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationRuleEngine"/> class.
        /// </summary>
        /// <param name="callerInformationService">The caller information service.</param>
        public CallerInformationRuleEngine(ICallerInformationService callerInformationService)
        {
            _callerInformationService = callerInformationService;
        }

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> GetCallerInformation(long callCenterHeaderID)
        {
            return _callerInformationService.GetCallerInformation(callCenterHeaderID);
        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="infoModel">The information model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> AddCallerInformation(CallCenterProgressNoteModel infoModel)
        {
            return _callerInformationService.AddCallerInformation(infoModel);
        }

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="infoModel">The information model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateCallerInformation(CallCenterProgressNoteModel infoModel)
        {
            return _callerInformationService.UpdateCallerInformation(infoModel);
        }

        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateModifiedOn(CallCenterProgressNoteModel model)
        {
            return _callerInformationService.UpdateModifiedOn(model);
        }


        /// <summary>
        /// Gets the call center assessment responses by contact id.
        /// </summary>
        /// <param name="infoModel">The contact identifier.</param>
        /// <returns></returns>
        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponseByContactID(long contactID)
        {
            return _callerInformationService.GetCallCenterAssessmentResponseByContactID(contactID);
        }

        /// <summary>
        /// Gets the call center assessment response.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponse(long callCenterHeaderID, long assessmentID)
        {
            return _callerInformationService.GetCallCenterAssessmentResponse(callCenterHeaderID, assessmentID);
        }

        /// <summary>
        /// Adds the call center assessment response.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<CallCenterAssessmentResponseModel> AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel model)
        {
            return _callerInformationService.AddCallCenterAssessmentResponse(model);
        }
    }
}