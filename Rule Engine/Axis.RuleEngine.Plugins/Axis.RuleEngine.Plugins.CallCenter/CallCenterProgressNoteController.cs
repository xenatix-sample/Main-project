using Axis.Constant;
using Axis.Model;
using Axis.Model.Common;
using Axis.RuleEngine.CallCenter;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.CallCenter
{
    public class CallCenterProgressNoteController : BaseApiController
    {
         /// <summary>
        /// The call center progress note
        /// </summary>
        private readonly ICallCenterProgressNoteRuleEngine _callCenterProgressNoteRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallCenterProgressNoteController"/> class.
        /// </summary>
        /// <param name="callerInformationRuleEngine">The call center summary rule engine.</param>
        public CallCenterProgressNoteController(ICallCenterProgressNoteRuleEngine callerInformationRuleEngine)
        {
            _callCenterProgressNoteRuleEngine = callerInformationRuleEngine;
        }


        /// <summary>
        /// Gets the call center progress note.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Demography,CallCenterPermissionKey.CallCenter_LawLiaison, ChartPermissionKey.Chart_Chart_RecordedServices }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetCallCenterProgressNote(long callCenterHeaderID)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callCenterProgressNoteRuleEngine.GetCallCenterProgressNote(callCenterHeaderID), Request);
        }



        /// <summary>
        /// Adds the call center progress note.
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The callCenterProgressNoteModel.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions = new string[] { Permission.Create ,Permission.Update})]
        [HttpPost]
        public IHttpActionResult AddCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callCenterProgressNoteRuleEngine.AddCallCenterProgressNote(callCenterProgressNoteModel), Request);
        }


        /// <summary>
        /// Updates the call center progress note.
        /// </summary>
        /// <param name="callCenterProgressNoteModel">The callCenterProgressNoteModel.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Demography , CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateCallCenterProgressNote(CallCenterProgressNoteModel callCenterProgressNoteModel)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callCenterProgressNoteRuleEngine.UpdateCallCenterProgressNote(callCenterProgressNoteModel), Request);
        }
    }
}
