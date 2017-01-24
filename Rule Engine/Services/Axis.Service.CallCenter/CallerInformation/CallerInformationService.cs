using Axis.Configuration;
using Axis.Model;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.CallCenter.CallerInformation
{
    public class CallerInformationService : ICallerInformationService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "CallerInformation/";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationService"/> class.
        /// </summary>
        public CallerInformationService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public CallerInformationService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> GetCallerInformation(long callCenterHeaderID)
        {
            const string apiUrl = BaseRoute + "GetCallerInformation";
            var requestId = new NameValueCollection { { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<CallCenterProgressNoteModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> AddCallerInformation(CallCenterProgressNoteModel model)
        {
            const string apiUrl = BaseRoute + "AddCallerInformation";
            return _communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model, apiUrl);
        }

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateCallerInformation(CallCenterProgressNoteModel model)
        {
            const string apiUrl = BaseRoute + "UpdateCallerInformation";
            return _communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model, apiUrl);
        }

        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name = "callCenterHeaderID" > The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateModifiedOn(CallCenterProgressNoteModel model)
        {
            const string apiUrl = BaseRoute + "UpdateModifiedOn";
            return _communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model, apiUrl);
       }

        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponse(long callCenterHeaderID, long assessmentID)
        {
            const string apiUrl = BaseRoute + "GetCallCenterAssessmentResponse";
            var requestId = new NameValueCollection { { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) }, { "assessmentID", assessmentID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<CallCenterAssessmentResponseModel>>(requestId, apiUrl);
        }

        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponseByContactID(long contactID)
        {
            const string apiUrl = BaseRoute + "GetCallCenterAssessmentResponseByContactID";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<CallCenterAssessmentResponseModel>>(requestId, apiUrl);
        }

        public Response<CallCenterAssessmentResponseModel> AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel model)
        {
            const string apiUrl = BaseRoute + "AddCallCenterAssessmentResponse";
            return _communicationManager.Post<CallCenterAssessmentResponseModel, Response<CallCenterAssessmentResponseModel>>(model, apiUrl);
        }
    }
}