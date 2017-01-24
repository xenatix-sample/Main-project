using System;
using Axis.Model.Common;
using Axis.Plugins.Clinical.Models;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical
{
    public interface INoteRepository
    {
        /// <summary>
        /// Gets the contact Notes
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        Response<NoteViewModel> GetNotes(long ContactID);

        /// <summary>
        /// Gets the contact Notes async
        /// </summary>
        /// <param name="ContactID"></param>
        /// <returns></returns>
        Task<Response<NoteViewModel>> GetNotesAsync(long ContactID);

        /// <summary>
        /// Adds the note.
        /// </summary>
        /// <param name="note">The Note.</param>
        /// <returns></returns>
        Response<NoteViewModel> AddNote(NoteViewModel note);

        /// <summary>
        /// Updates the Note.
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        Response<NoteViewModel> UpdateNote(NoteViewModel note);

        /// <summary>
        /// Update note for NoteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        Response<NoteDetailsViewModel> UpdateNoteDetails(NoteDetailsViewModel note);

        /// <summary>
        /// Deletes the Note.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<NoteViewModel> DeleteNote(long id, DateTime modifiedOn);

    }
}
