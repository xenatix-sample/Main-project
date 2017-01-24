using Axis.Model;
using Axis.Model.Common;
using Axis.Service.CallCenter;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.CallCenter
{
    public class CallCenterProgressNoteRuleEngine : ICallCenterProgressNoteRuleEngine
    {
        private readonly ICallCenterProgressNoteService _callerInformationService;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteRuleEngine"/> class.
        /// </summary>
        /// <param name="callerInformationService">The call center progress note service.</param>
        public CallCenterProgressNoteRuleEngine(ICallCenterProgressNoteService callerInformationService)
        {
            _callerInformationService = callerInformationService;
        }


        /// <summary>
        /// Gets the call center progress note.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> GetCallCenterProgressNote(long callCenterHeaderID)
        {
            return _callerInformationService.GetCallCenterProgressNote(callCenterHeaderID);
        }


        /// <summary>
        /// Adds the call center progress note.
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The call center progress note.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> AddCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel)
        {
            return _callerInformationService.AddCallCenterProgressNote(callCenterProgressNoteModel);
        }

        /// <summary>
        /// Updates the call center progress note.
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The call center progress note.</param>
        /// <returns></returns>
        public Response<CallCenterProgressNoteModel> UpdateCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel)
        {
            return _callerInformationService.UpdateCallCenterProgressNote(callCenterProgressNoteModel);
        }
    }
}
