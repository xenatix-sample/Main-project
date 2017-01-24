using Axis.Model.Common;
using Axis.Plugins.CallCenter.Repository;
using Axis.PresentationEngine.Helpers.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.Plugins.CallCenter.ApiControllers
{
    public class CallCenterProgressNoteController : BaseApiController
    {
          /// <summary>
        /// The caller information repository
        /// </summary>
        private readonly ICallCenterProgressNoteRepository _CallCenterProgressNoteRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteController"/> class.
        /// </summary>
        /// <param name="CallCenterProgressNoteRepository">The call center summary repository.</param>
        public CallCenterProgressNoteController(ICallCenterProgressNoteRepository CallCenterProgressNoteRepository)
        {
            _CallCenterProgressNoteRepository = CallCenterProgressNoteRepository;
        }



        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="CallCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<CallCenterProgressNoteViewModel> GetCallCenterProgressNote(long CallCenterHeaderID)
        {
            return _CallCenterProgressNoteRepository.GetCallCenterProgressNote(CallCenterHeaderID);

        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<CallCenterProgressNoteViewModel> AddCallCenterProgressNote(CallCenterProgressNoteViewModel model)
        {
            return _CallCenterProgressNoteRepository.AddCallCenterProgressNote(model);
        }


        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<CallCenterProgressNoteViewModel> UpdateCallCenterProgressNote(CallCenterProgressNoteViewModel model)
        {
            return _CallCenterProgressNoteRepository.UpdateCallCenterProgressNote(model);
        }
    }
}
