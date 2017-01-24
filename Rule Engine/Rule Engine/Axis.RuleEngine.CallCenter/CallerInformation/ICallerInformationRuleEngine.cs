using Axis.Model;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using System;

namespace Axis.RuleEngine.CallCenter.CallerInformation
{
    public interface ICallerInformationRuleEngine
    {
        #region Caller Information

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> GetCallerInformation(long callCenterHeaderID);

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="infoModel">The information model.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> AddCallerInformation(CallCenterProgressNoteModel infoModel);

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="infoModel">The information model.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> UpdateCallerInformation(CallCenterProgressNoteModel infoModel);

        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteModel> UpdateModifiedOn(CallCenterProgressNoteModel model);
       

        #endregion Caller Information

        #region Call Center Assessment Response

        /// <summary>
        /// Gets the call center assessment response.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponse(long callCenterHeaderID, long assessmentID);

        /// <summary>
        /// Gets the call center assessment response.
        /// </summary>
        /// <param name="callCenterHeaderID">The contact identifier.</param>
        /// <returns></returns>
        Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponseByContactID(long contactID);

        /// <summary>
        /// Adds the call center assessment response.
        /// </summary>
        /// <param name="callCenterAssessmentResponse">The call center assessment response.</param>
        /// <returns></returns>
        Response<CallCenterAssessmentResponseModel> AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel callCenterAssessmentResponse);
   
        #endregion Call Center Assessment Response
    }
}