using Axis.Configuration;
using Axis.Helpers;
using Axis.Model;
using Axis.Model.Common;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using Axis.Plugins.CallCenter.Translator;
using Axis.Constant;

namespace Axis.Plugins.CallCenter.Repository
{
    public class CallCenterProgressNoteRepository : ICallCenterProgressNoteRepository
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "CallCenterProgressNote/";

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCallCenterProgressNote"/> class.
        /// </summary>
        public CallCenterProgressNoteRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UpdateCallCenterProgressNote"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public CallCenterProgressNoteRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the call center progress note.
        /// </summary>
        /// <param name="HeaderID">The search string.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteViewModel> GetCallCenterProgressNote(long callCenterHeaderID)
        {
            const string apiUrl = baseRoute + "GetCallCenterProgressNote";
            var param = new NameValueCollection { { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<CallCenterProgressNoteModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the call center progress note.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteViewModel> AddCallCenterProgressNote(CallCenterProgressNoteViewModel model)
        {
            const string apiUrl = baseRoute + "AddCallCenterProgressNote";
            var response = _communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the call center progress note.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteViewModel> UpdateCallCenterProgressNote(CallCenterProgressNoteViewModel model)
        {
            const string apiUrl = baseRoute + "UpdateCallCenterProgressNote";
            var response = _communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(model.ToModel(), apiUrl);
            return response.ToViewModel();
        }
    }
}
