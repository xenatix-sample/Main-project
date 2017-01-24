using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.CallCenter;
using Axis.Model;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.CallCenter
{
    public class CallCenterProgressNoteController : BaseApiController
    {
           /// <summary>
        /// The _call center progress note data provider
        /// </summary>
        private readonly ICallCenterProgressNoteDataProvider _CallCenterProgressNoteDataProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteController"/> class.
        /// </summary>
        /// <param name="CallCenterProgressNoteDataProvider">The call center progress note data provider.</param>
        public CallCenterProgressNoteController(ICallCenterProgressNoteDataProvider CallCenterProgressNoteDataProvider)
        {
            _CallCenterProgressNoteDataProvider = CallCenterProgressNoteDataProvider;
        }


        /// <summary>
        /// Gets the call center progress note.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetCallCenterProgressNote(long callCenterHeaderID)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_CallCenterProgressNoteDataProvider.GetCallCenterProgressNote(callCenterHeaderID), Request);
        }


        /// <summary>
        /// Adds the call center progress note.
        /// </summary>
        /// <param name="model">The call center progress note.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNote)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_CallCenterProgressNoteDataProvider.AddCallCenterProgressNote(callCenterProgressNote), Request);
        }


        /// <summary>
        /// Updates the call center progress note.
        /// </summary>
        /// <param name="model">The call center progress note.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNote)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_CallCenterProgressNoteDataProvider.UpdateCallCenterProgressNote(callCenterProgressNote), Request);
        }
    }
}
