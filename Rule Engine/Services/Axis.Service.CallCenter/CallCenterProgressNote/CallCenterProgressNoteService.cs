using Axis.Configuration;
using Axis.Model;
using Axis.Model.Common;
using Axis.Security;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Service.CallCenter
{
    public class CallCenterProgressNoteService : ICallCenterProgressNoteService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "CallCenterProgressNote/";

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteService"/> class.
        /// </summary>
        public CallCenterProgressNoteService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteService"/> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public CallCenterProgressNoteService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }


        /// <summary>
        /// Gets the call center progress note .
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> GetCallCenterProgressNote(long callCenterHeaderID)
        {
            const string apiUrl = BaseRoute + "GetCallCenterProgressNote";
            var requestId = new NameValueCollection { { "callCenterHeaderID", callCenterHeaderID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<CallCenterProgressNoteModel>>(requestId, apiUrl);
        }


        /// <summary>
        /// Adds the call center progress note .
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The call center progress note model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> AddCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel)
        {
            const string apiUrl = BaseRoute + "AddCallCenterProgressNote";
            return _communicationManager.Post<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(callCenterProgressNoteModel, apiUrl);
        }


        /// <summary>
        /// Updates the call center progress note .
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The call center progress note model.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel)
        {
            const string apiUrl = BaseRoute + "UpdateCallCenterProgressNote";
            return _communicationManager.Put<CallCenterProgressNoteModel, Response<CallCenterProgressNoteModel>>(callCenterProgressNoteModel, apiUrl);
        }
    }
}
