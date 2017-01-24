using System;
using Axis.Model.Common;
using Axis.Model.ECI;

namespace Axis.DataProvider.ECI
{
    public interface INoteHeaderDataProvider
    {
        /// <summary>
        /// Gets the note headers.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> GetNoteHeaders(long contactID, long moduleID, int noteTypeID);

        /// <summary>
        /// Gets the note header.
        /// </summary>
        /// <param name="NoteHeaderId">The note header identifier.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> GetNoteHeader(long noteHeaderId);

        /// <summary>
        /// Adds the note header.
        /// </summary>
        /// <param name="NoteHeader">The note header.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> AddNoteHeader(NoteHeaderModel noteHeader);

        /// <summary>
        /// Updates the note header.
        /// </summary>
        /// <param name="NoteHeader">The note header.</param>
        /// <returns></returns>
        Response<NoteHeaderModel> UpdateNoteHeader(NoteHeaderModel noteHeader);

        /// <summary>
        /// Deletes the note header.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<NoteHeaderModel> DeleteNoteHeader(long Id, DateTime modifiedOn);
    }
}
