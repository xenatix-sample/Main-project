using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.Plugins.CallCenter.Models;
using Axis.Plugins.CallCenter.Translator;
using Axis.Service;
using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.Plugins.CallCenter.Repository.CallerInformation
{
    public class CallerInformationRepository : ICallerInformationRepository
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "CallerInformation/";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationRepository"/> class.
        /// </summary>
        public CallerInformationRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationRepository"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public CallerInformationRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the Caller Information.
        /// </summary>
        /// <param name="HeaderID">The search string.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteViewModel> GetCallerInformation(long callCenterHeaderID)
        {
            const string apiUrl = baseRoute + "GetCallerInformation";
            var param = new NameValueCollection { { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<CallCenterProgressNoteModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteViewModel> AddCallerInformation(CallCenterProgressNoteViewModel model)
        {
            const string apiUrl = baseRoute + "AddCallerInformation";
            var response = _communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }
        
        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteViewModel> UpdateCallerInformation(CallCenterProgressNoteViewModel model)
        {
            const string apiUrl = baseRoute + "UpdateCallerInformation";
            var response = _communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }


        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name = "callCenterHeaderID" > The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteViewModel> UpdateModifiedOn(CallCenterProgressNoteViewModel model)
        {
            const string apiUrl = baseRoute + "UpdateModifiedOn";
            var response = _communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }




        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponse(long callCenterHeaderID, long assessmentID)
        {
            const string apiUrl = baseRoute + "GetCallCenterAssessmentResponse";
            var requestId = new NameValueCollection { { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) }, { "assessmentID", assessmentID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<CallCenterAssessmentResponseModel>>(requestId, apiUrl);
        }

        public Response<CallCenterAssessmentResponseModel> GetCallCenterAssessmentResponseByContactID(long contactID)
        {
            const string apiUrl = baseRoute + "GetCallCenterAssessmentResponseByContactID";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) }};

            return _communicationManager.Get<Response<CallCenterAssessmentResponseModel>>(requestId, apiUrl);
        }

        public Response<CallCenterAssessmentResponseModel> AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel model)
        {
            const string apiUrl = baseRoute + "AddCallCenterAssessmentResponse";
            return _communicationManager.Post<CallCenterAssessmentResponseModel, Response<CallCenterAssessmentResponseModel>>(model, apiUrl);
        }
    }
}
