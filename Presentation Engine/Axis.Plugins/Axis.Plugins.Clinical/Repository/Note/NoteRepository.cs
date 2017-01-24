using System;
using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;
using Axis.Plugins.Clinical.Translator;


namespace Axis.Plugins.Clinical
{
    public class NoteRepository : INoteRepository
    {
        /// <summary>
        /// The communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Note/";

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteRepository" /> class.
        /// </summary>
        public NoteRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NoteRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public NoteRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        /// <summary>
        /// Gets the notes
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<NoteViewModel> GetNotes(long ContactID)
        {
            return GetNotesAsync(ContactID).Result;
        }

        /// <summary>
        /// Gets the notes asynchronously
        /// </summary>
        /// <param name="ContactID">The contact identifier.</param>
        /// <returns></returns>
        public async Task<Response<NoteViewModel>> GetNotesAsync(long ContactID)
        {
            string apiUrl = baseRoute + "GetNotes";
            var parameters = new NameValueCollection { { "ContactId", ContactID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<NoteModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Add note
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public Response<NoteViewModel> AddNote(NoteViewModel note)
        {
            string apiUrl = baseRoute + "AddNote";
            var response = _communicationManager.Post<NoteModel, Response<NoteModel>>(note.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Update Note
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public Response<NoteViewModel> UpdateNote(NoteViewModel note)
        {
            string apiUrl = baseRoute + "UpdateNote";
            var response = _communicationManager.Put<NoteModel, Response<NoteModel>>(note.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Update note for NoteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public Response<NoteDetailsViewModel> UpdateNoteDetails(NoteDetailsViewModel note)
        {
            string apiUrl = baseRoute + "UpdateNoteDetails";
            var response = _communicationManager.Put<NoteDetailsModel, Response<NoteDetailsModel>>(note.ToModel(), apiUrl);
            return response.ToViewModel();
        }

        /// <summary>
        /// Delete Note
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<NoteViewModel> DeleteNote(long Id, DateTime modifiedOn)
        {
            string apiUrl = baseRoute;
            var param = new NameValueCollection
            {
                {"Id", Id.ToString()},
                {"modifiedOn", modifiedOn.ToString(CultureInfo.InvariantCulture)}
            };

            return _communicationManager.Delete<Response<NoteModel>>(param, apiUrl).ToViewModel();
        }
    }
}
