using System;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.ECI
{
    public class ProgressNoteService : IProgressNoteService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "ProgressNote/";

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressNoteService"/> class.
        /// </summary>
        public ProgressNoteService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> GetProgressNotes(int noteTypeID, long contactID)
        {
            string apiUrl = baseRoute + "GetProgressNotes";
            var param = new NameValueCollection { { "noteTypeID", noteTypeID.ToString(CultureInfo.InvariantCulture) },
                                                  { "contactID", contactID.ToString(CultureInfo.InvariantCulture) }};
            return _communicationManager.Get<Response<NoteHeaderModel>>(param, apiUrl);
        }

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> GetProgressNote(long progressNoteId)
        {
            string apiUrl = baseRoute + "GetProgressNote";
            var param = new NameValueCollection { { "progressNoteId", progressNoteId.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Get<Response<NoteHeaderModel>>(param, apiUrl);
        }

        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> AddProgressNote(NoteHeaderModel noteHeader)
        {
            const string apiUrl = baseRoute + "AddProgressNote";
            return _communicationManager.Post<NoteHeaderModel, Response<NoteHeaderModel>>(noteHeader, apiUrl);
        }

        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> UpdateProgressNote(NoteHeaderModel noteHeader)
        {
            const string apiUrl = baseRoute + "UpdateProgressNote";
            return _communicationManager.Put<NoteHeaderModel, Response<NoteHeaderModel>>(noteHeader, apiUrl);
        }

        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> DeleteProgressNote(long Id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute + "DeleteProgressNote";
            var param = new NameValueCollection
            {
                { "Id", Id.ToString() },
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };
            return _communicationManager.Delete<Response<NoteHeaderModel>>(param, apiUrl);
        }

    }
}
