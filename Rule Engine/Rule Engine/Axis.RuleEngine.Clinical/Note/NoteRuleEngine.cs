using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.Service.Clinical;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Clinical
{
    public class NoteRuleEngine : INoteRuleEngine
    {
        
        readonly INoteService _noteService;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="noteService"></param>
        public NoteRuleEngine(INoteService noteService)
        {
            _noteService = noteService;
        }

        /// <summary>
        /// To get list of notes for contact
        /// </summary>
        /// <param name="contactID"></param>
        /// <returns></returns>
        public Response<NoteModel> GetNotes(long contactID)
        {
            return _noteService.GetNotes(contactID);
        }

        /// <summary>
        /// To add new note for contact
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Response<NoteModel> AddNote(NoteModel note)
        {
            return _noteService.AddNote(note);
        }

        /// <summary>
        /// To update note for contact
        /// </summary>
        /// <param name="note"></param>
        /// <returns></returns>
        public Response<NoteModel> UpdateNote(NoteModel note)
        {
            return _noteService.UpdateNote(note);
        }

        /// <summary>
        /// Update note for NoteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        public Response<NoteDetailsModel> UpdateNoteDetails(NoteDetailsModel note)
        {
            return _noteService.UpdateNoteDetails(note);
        }

        /// <summary>
        /// To remove note for contact
        /// </summary>
        /// <param name="Id"></param>
        /// <returns></returns>
        public Response<NoteModel> DeleteNote(long Id, DateTime modifiedOn)
        {
            return _noteService.DeleteNote(Id, modifiedOn);
        }
    }
}
