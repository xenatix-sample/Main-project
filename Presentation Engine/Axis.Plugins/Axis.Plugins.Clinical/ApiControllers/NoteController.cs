using System;
using Axis.Plugins.Clinical.Models;
using System.Web.Http;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.ApiControllers
{
    public class NoteController : BaseApiController
    {
        readonly INoteRepository _noteRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="noteRepository"></param>
        public NoteController(INoteRepository noteRepository)
        {
            _noteRepository = noteRepository;
        }

        /// <summary>
        /// Gets the contact Notes
        /// </summary>
        /// <param name="ContactID">The contact Id.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<NoteViewModel> GetNotes(long ContactID)
        {
            return _noteRepository.GetNotes(ContactID);
        }

        /// <summary>
        /// Gets the list of Notes
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public async Task<Response<NoteViewModel>> GetNotesAsync(long ContactID)
        {
            var result = await _noteRepository.GetNotesAsync(ContactID);
            return result;
        }

        /// <summary>
        /// Adds the note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<NoteViewModel> AddNote(NoteViewModel note)
        {
            note.TakenTime = note.TakenTime.ToUniversalTime();
            return _noteRepository.AddNote(note);
        }

        /// <summary>
        /// Updates the Note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<NoteViewModel> UpdateNote(NoteViewModel note)
        {
            note.TakenTime = note.TakenTime.ToUniversalTime();
            return _noteRepository.UpdateNote(note);
        }

        /// <summary>
        /// Updates the Note for NoteID.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<NoteDetailsViewModel> UpdateNoteDetails(NoteDetailsViewModel note)
        {
            return _noteRepository.UpdateNoteDetails(note);
        }

        /// <summary>
        /// Deletes the Note.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<NoteViewModel> DeleteNote(long Id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _noteRepository.DeleteNote(Id, modifiedOn);
        }
    }
}
