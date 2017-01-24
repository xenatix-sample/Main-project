using System;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.Service.ECI;

namespace Axis.RuleEngine.ECI
{
    public class ProgressNoteRuleEngine : IProgressNoteRuleEngine
    {
        /// <summary>
        /// The _progress note service
        /// </summary>
        private readonly IProgressNoteService _progressNoteService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressNoteRuleEngine"/> class.
        /// </summary>
        /// <param name="progressNoteService">The progress note service.</param>
        public ProgressNoteRuleEngine(IProgressNoteService progressNoteService)
        {
            _progressNoteService = progressNoteService;
        }

        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> GetProgressNotes(int noteTypeID, long contactID)
        {
            return _progressNoteService.GetProgressNotes(noteTypeID, contactID);
        }

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public Response<NoteHeaderModel> GetProgressNote(long progressNoteId)
        {
            return _progressNoteService.GetProgressNote(progressNoteId);
        }

        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> AddProgressNote(NoteHeaderModel noteHeader)
        {
            return _progressNoteService.AddProgressNote(noteHeader);
        }

        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        public Response<NoteHeaderModel> UpdateProgressNote(NoteHeaderModel noteHeader)
        {
            return _progressNoteService.UpdateProgressNote(noteHeader);
        }

        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<NoteHeaderModel> DeleteProgressNote(long Id, DateTime modifiedOn)
        {
            return _progressNoteService.DeleteProgressNote(Id, modifiedOn);
        }
    }
}
