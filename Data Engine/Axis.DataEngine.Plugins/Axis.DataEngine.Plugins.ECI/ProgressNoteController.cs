using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI;
using Axis.Model.Common;
using Axis.Model.ECI;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.ECI
{
    public class ProgressNoteController : BaseApiController
    {
        /// <summary>
        /// The _progress note rule engine
        /// </summary>
        private readonly IProgressNoteDataProvider _progressNoteDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProgressNoteController"/> class.
        /// </summary>
        /// <param name="progressNoteDataProvider">The progress note data provider.</param>
        public ProgressNoteController(IProgressNoteDataProvider progressNoteDataProvider)
        {
            _progressNoteDataProvider = progressNoteDataProvider;
        }

        /// <summary>
        /// Gets the progress notes.
        /// </summary>
        /// <param name="noteTypeID">The note type identifier.</param>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProgressNotes(int noteTypeID, long contactID)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteDataProvider.GetProgressNotes(noteTypeID, contactID), Request);
        }

        /// <summary>
        /// Gets the progress note.
        /// </summary>
        /// <param name="progressNoteId">The progress note identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetProgressNote(long progressNoteId)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteDataProvider.GetProgressNote(progressNoteId), Request);
        }

        /// <summary>
        /// Adds the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddProgressNote(NoteHeaderModel noteHeader)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteDataProvider.AddProgressNote(noteHeader), Request);
        }

        /// <summary>
        /// Updates the progress note.
        /// </summary>
        /// <param name="noteHeader">The note header.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateProgressNote(NoteHeaderModel noteHeader)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteDataProvider.UpdateProgressNote(noteHeader), Request);
        }

        /// <summary>
        /// Deletes the progress note.
        /// </summary>
        /// <param name="Id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteProgressNote(long Id, DateTime modifiedOn)
        {
            return new HttpResult<Response<NoteHeaderModel>>(_progressNoteDataProvider.DeleteProgressNote(Id, modifiedOn), Request);
        }
    }
}
