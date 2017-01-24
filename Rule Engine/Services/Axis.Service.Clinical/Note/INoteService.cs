using System;
using Axis.Model.Clinical;
using Axis.Model.Common;

namespace Axis.Service.Clinical
{
    public interface INoteService
    {
        /// <summary>
        /// To get the list of notes
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        Response<NoteModel> GetNotes(long contactID);

        /// <summary>
        /// To add note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        Response<NoteModel> AddNote(NoteModel note);
                
        /// <summary>
        /// To update note
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        Response<NoteModel> UpdateNote(NoteModel note);

        /// <summary>
        /// Update note for NoteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        Response<NoteDetailsModel> UpdateNoteDetails(NoteDetailsModel note);

        /// <summary>
        /// To remove note
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<NoteModel> DeleteNote(long Id, DateTime modifiedOn);

    }
}
