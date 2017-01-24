using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Plugins.ECI.Models;
using Axis.Plugins.ECI.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;


namespace Axis.Plugins.ECI
{
    public class ProgressNoteRepository : IProgressNoteRepository
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
        /// Constructor
        /// </summary>
        public ProgressNoteRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public ProgressNoteRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
       
        public Response<NoteHeaderViewModel> GetProgressNotes(int noteTypeID, long contactID)
        {
            string apiUrl = baseRoute + "GetProgressNotes";
            var param = new NameValueCollection { { "noteTypeID", noteTypeID.ToString(CultureInfo.InvariantCulture) },
                                                   { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<NoteHeaderModel>>(param, apiUrl);
            return response.ToViewModel();
        }
        
        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<NoteHeaderViewModel> GetProgressNote(long progressNoteId)
        {
            string apiUrl = baseRoute + "GetProgressNote";
            var param = new NameValueCollection { { "progressNoteId", progressNoteId.ToString(CultureInfo.InvariantCulture) } };
            var response = _communicationManager.Get<Response<NoteHeaderModel>>(param, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        
        public Response<NoteHeaderViewModel> AddProgressNote(NoteHeaderViewModel noteHeader)
        {
            const string apiUrl = baseRoute + "AddProgressNote";
            var response = _communicationManager.Post<NoteHeaderModel, Response<NoteHeaderModel>>(noteHeader.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
       
        public Response<NoteHeaderViewModel> UpdateProgressNote(NoteHeaderViewModel noteHeader)
        {
            const string apiUrl = baseRoute + "UpdateProgressNote";
            var response = _communicationManager.Put<NoteHeaderModel, Response<NoteHeaderModel>>(noteHeader.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
      
        public Response<NoteHeaderViewModel> DeleteProgressNote(long Id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute + "DeleteProgressNote";
            var param = new NameValueCollection { { "Id", Id.ToString() }, { "modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture) } };
            return _communicationManager.Delete<Response<NoteHeaderViewModel>>(param, apiUrl);
        }
    }
}
