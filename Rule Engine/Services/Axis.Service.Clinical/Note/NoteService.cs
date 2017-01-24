using System;
using Axis.Configuration;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Clinical
{
    public class NoteService : INoteService
    {
        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "note/";

        /// <summary>
        /// Constructor
        /// </summary>
        public NoteService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="token"></param>
        public NoteService(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// To get the list of notes
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        public Response<NoteModel> GetNotes(long contactID)
        {
            const string apiUrl = BaseRoute + "GetNotes";
            var requestId = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<NoteModel>>(requestId, apiUrl);
        }

        /// <summary>
        /// To add note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Response<NoteModel> AddNote(NoteModel note)
        {
            const string apiUrl = BaseRoute + "AddNote";
            return _communicationManager.Post<NoteModel, Response<NoteModel>>(note, apiUrl);
        }

        /// <summary>
        /// To update note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Response<NoteModel> UpdateNote(NoteModel note)
        {
            const string apiUrl = BaseRoute + "UpdateNote";
            return _communicationManager.Put<NoteModel, Response<NoteModel>>(note, apiUrl);
        }

        /// <summary>
        /// Update note for NoteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public Response<NoteDetailsModel> UpdateNoteDetails(NoteDetailsModel note)
        {
            const string apiUrl = BaseRoute + "UpdateNoteDetails";
            return _communicationManager.Put<NoteDetailsModel, Response<NoteDetailsModel>>(note, apiUrl);
        }

        /// <summary>
        /// To remove note
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Response<NoteModel> DeleteNote(long Id, DateTime modifiedOn)
        {
            const string apiUrl = BaseRoute + "DeleteNote";
            var requestId = new NameValueCollection { { "Id", Id.ToString(CultureInfo.InvariantCulture) }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Delete<Response<NoteModel>>(requestId, apiUrl);
        }
    }
}
