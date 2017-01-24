using System;
using Axis.Model.Common;
using Axis.Model.ECI;
using Axis.RuleEngine.ECI;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Plugins.ECI
{
    public class ProgressNoteController : BaseApiController
    {
        /// <summary>
        /// The _progress note rule engine
        /// </summary>
        private readonly IProgressNoteRuleEngine _progressNoteRuleEngine;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="ifspRepository"></param>
        public ProgressNoteController(IProgressNoteRuleEngine progressNoteRuleEngine)
        {
            _progressNoteRuleEngine = progressNoteRuleEngine;
        }

        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_ProgressNote_ProgressNote, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetProgressNotes(int noteTypeID, long contactID)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteRuleEngine.GetProgressNotes(noteTypeID, contactID), Request);
        }

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_ProgressNote_ProgressNote, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetProgressNote(long progressNoteId)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteRuleEngine.GetProgressNote(progressNoteId), Request);
        }

        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_ProgressNote_ProgressNote, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddProgressNote(NoteHeaderModel noteHeader)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteRuleEngine.AddProgressNote(noteHeader), Request);
        }

        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_ProgressNote_ProgressNote, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateProgressNote(NoteHeaderModel noteHeader)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteRuleEngine.UpdateProgressNote(noteHeader), Request);
        }

        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id">The identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ECIPermissionKey.ECI_ProgressNote_ProgressNote, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteProgressNote(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteRuleEngine.DeleteProgressNote(Id, modifiedOn), Request);
        }
    }
}
