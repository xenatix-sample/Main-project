using System;
using Axis.Plugins.ECI.Models;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.ECI.ApiControllers
{
    public class ProgressNoteController : BaseApiController
    {
        /// <summary>
        /// The _progress note repository
        /// </summary>
        private readonly IProgressNoteRepository _progressNoteRepository;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ifspRepository"></param>
        public ProgressNoteController(IProgressNoteRepository progressNoteRepository)
        {
            _progressNoteRepository = progressNoteRepository;
        }

        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<NoteHeaderViewModel> GetProgressNotes(int noteTypeID, long contactID)
        {
            return _progressNoteRepository.GetProgressNotes(noteTypeID, contactID);
        }

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<NoteHeaderViewModel> GetProgressNote(long progressNoteId)
        {
            return _progressNoteRepository.GetProgressNote(progressNoteId);
        }

        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<NoteHeaderViewModel> AddProgressNote(NoteHeaderViewModel noteHeader)
        {
            return _progressNoteRepository.AddProgressNote(noteHeader);
        }

        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<NoteHeaderViewModel> UpdateProgressNote(NoteHeaderViewModel noteHeader)
        {
            return _progressNoteRepository.UpdateProgressNote(noteHeader);
        }

        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public Response<NoteHeaderViewModel> DeleteProgressNote(long Id, DateTime modifiedOn)
        {
            modifiedOn = modifiedOn.ToUniversalTime();
            return _progressNoteRepository.DeleteProgressNote(Id, modifiedOn);
        }
    }
}
