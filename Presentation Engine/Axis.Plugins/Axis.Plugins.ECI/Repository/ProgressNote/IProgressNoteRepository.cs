using Axis.Model.Common;
using Axis.Plugins.ECI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ECI
{
    public interface IProgressNoteRepository
    {
        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<NoteHeaderViewModel> GetProgressNotes(int noteTypeID, long contactID);

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        Response<NoteHeaderViewModel> GetProgressNote(long progressNoteId);
        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        Response<NoteHeaderViewModel> AddProgressNote(NoteHeaderViewModel noteHeader);
        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        Response<NoteHeaderViewModel> UpdateProgressNote(NoteHeaderViewModel noteHeader);
        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<NoteHeaderViewModel> DeleteProgressNote(long Id, DateTime modifiedOn);
    }
}
