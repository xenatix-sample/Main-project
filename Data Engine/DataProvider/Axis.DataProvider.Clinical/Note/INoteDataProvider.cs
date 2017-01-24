using Axis.Model.Clinical;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Clinical
{
    public interface INoteDataProvider
    {
        /// <summary>
        /// Get list of notes for contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        Response<NoteModel> GetNotes(long contactID);
        
        /// <summary>
        /// Add note for contact
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        Response<NoteModel> AddNote(NoteModel note);

        /// <summary>
        /// Update note
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
        /// Remove note for contact
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<NoteModel> DeleteNote(long Id, DateTime modifiedOn);
    }
}
