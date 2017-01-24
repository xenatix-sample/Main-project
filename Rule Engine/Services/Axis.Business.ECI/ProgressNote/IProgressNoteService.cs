using System;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.Service.ECI
{
    public interface IProgressNoteService
    {
        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> GetProgressNotes(int noteTypeID, long contactID);

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> GetProgressNote(long progressNoteId);
        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> AddProgressNote(NoteHeaderModel noteHeader);
        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note Header.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> UpdateProgressNote(NoteHeaderModel noteHeader);
        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<NoteHeaderModel> DeleteProgressNote(long Id, DateTime modifiedOn);
    }
}
