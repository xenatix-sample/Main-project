using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using System;
using System.Threading.Tasks;

namespace Axis.Plugins.CallCenter.Repository.CallerInformation
{
    /// <summary>
    /// Repository for caller information
    /// </summary>
    public interface ICallerInformationRepository
    {

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteViewModel> GetCallerInformation(long callCenterHeaderID);

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteViewModel> AddCallerInformation(CallCenterProgressNoteViewModel model);

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteViewModel> UpdateCallerInformation(CallCenterProgressNoteViewModel model);


        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name = "callCenterHeaderID" > The call center header identifier.</param>
        /// <returns></returns>
        Response<CallCenterProgressNoteViewModel> UpdateModifiedOn(CallCenterProgressNoteViewModel model);

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

    }
}
