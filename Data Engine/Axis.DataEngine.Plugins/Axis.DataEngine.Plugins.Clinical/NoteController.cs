using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Clinical;
using Axis.Model.Clinical;
using Axis.Model.Common;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Clinical
{
    public class NoteController : BaseApiController
    {
        readonly INoteDataProvider _noteDataProvider;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="noteDataProvider"></param>
        public NoteController(INoteDataProvider noteDataProvider)
        {
            this._noteDataProvider = noteDataProvider;
        }

        /// <summary>
        /// To get Note list
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetNotes(long contactID)
        {
            return new HttpResult<Response<NoteModel>>(_noteDataProvider.GetNotes(contactID), Request);
        }

        /// <summary>
        /// To add note
        /// </summary>
        /// <param name="NoteModel">Note Model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddNote(NoteModel noteModel)
        {
            return new HttpResult<Response<NoteModel>>(_noteDataProvider.AddNote(noteModel), Request);
        }

        /// <summary>
        /// To update note
        /// </summary>
        /// <param name="NoteModel">Note Model</param>
        /// <returns>Response of type NoteModel</returns>
        [HttpPut]
        public IHttpActionResult UpdateNote(NoteModel noteModel)
        {
            return new HttpResult<Response<NoteModel>>(_noteDataProvider.UpdateNote(noteModel), Request);
        }

        /// <summary>
        /// To update note for noteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateNoteDetails(NoteDetailsModel note)
        {
            return new HttpResult<Response<NoteDetailsModel>>(_noteDataProvider.UpdateNoteDetails(note), Request);
        }

        /// <summary>
        /// To remove note
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteNote(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<NoteModel>>(_noteDataProvider.DeleteNote(Id, modifiedOn), Request);
        }
    }
}
