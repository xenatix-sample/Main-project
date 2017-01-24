using System;
using Axis.Model.Clinical;
using Axis.Model.Common;
using Axis.RuleEngine.Clinical;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;
using Axis.Helpers.Validation;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    /// <summary>
    /// Controller for Note
    /// </summary>
    public class NoteController : BaseApiController
    {
        readonly INoteRuleEngine _noteRuleEngine;

        public NoteController(INoteRuleEngine noteRuleEngine)
        {
            this._noteRuleEngine = noteRuleEngine;
        }

        /// <summary>
        /// Get note list for contact
        /// </summary>
        /// <param name="contactID">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Note_Note, Permission = Permission.Read)]
        public IHttpActionResult GetNotes(long contactID)
        {
            return new HttpResult<Response<NoteModel>>(_noteRuleEngine.GetNotes(contactID), Request);
        }

        /// <summary>
        /// Add note for contact
        /// </summary>
        /// <param name="noteModel">note model</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Note_Note, Permission = Permission.Create)]
        public IHttpActionResult AddNote(NoteModel noteModel)
        {            
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<NoteModel>>(_noteRuleEngine.AddNote(noteModel), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<NoteModel>() { DataItems = new List<NoteModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<NoteModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Update note for model
        /// </summary>
        /// <param name="note">note model</param>
        /// <returns></returns>
        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Note_Note, Permission = Permission.Update)]
        public IHttpActionResult UpdateNote(NoteModel note)
        {            
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<NoteModel>>(_noteRuleEngine.UpdateNote(note), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<NoteModel>() { DataItems = new List<NoteModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<NoteModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Update note for NoteId
        /// </summary>
        /// <param name="note">The note.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Note_Note, Permission = Permission.Update)]
        public IHttpActionResult UpdateNoteDetails(NoteDetailsModel note)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<NoteDetailsModel>>(_noteRuleEngine.UpdateNoteDetails(note), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<NoteModel>() { DataItems = new List<NoteModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<NoteModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Delete note
        /// </summary>
        /// <param name="Id">Contact Id of note</param>
        /// <returns></returns>
        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Note_Note, Permission = Permission.Delete)]
        public IHttpActionResult DeleteNote(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<NoteModel>>(_noteRuleEngine.DeleteNote(Id, modifiedOn), Request);
        }
    }
}
