using Axis.Constant;
using Axis.Model;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.RuleEngine.CallCenter.CallerInformation;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.CallCenter
{

    public class CallerInformationController : BaseApiController
    {

        /// <summary>
        /// The caller information
        /// </summary>
        private readonly ICallerInformationRuleEngine _callerInformationRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="CallerInformationRuleEngine"/> class.
        /// </summary>
        /// <param name="callerInformationRuleEngine">The call center summary rule engine.</param>
        public CallerInformationController(ICallerInformationRuleEngine callerInformationRuleEngine)
        {
            _callerInformationRuleEngine = callerInformationRuleEngine;
        }

        /// <summary>
        /// Gets the caller information.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [Authorization(Modules = new string[] { Module.Benefits, Module.Consents, Module.CrisisLine, Module.Registration, Module.ECI, Module.General, Module.Intake, Module.LawLiaison, Module.SiteAdministration }, PermissionKeys = new string[] { ChartPermissionKey.Chart_Chart_RecordedServices, CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_LawLiaison, ChartPermissionKey.Chart_Chart_Assessment }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetCallerInformation(long callCenterHeaderID)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationRuleEngine.GetCallerInformation(callCenterHeaderID), Request);
        }

        /// <summary>
        /// Adds the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddCallerInformation(CallCenterProgressNoteModel model)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationRuleEngine.AddCallerInformation(model), Request);
        }

        /// <summary>
        /// Updates the caller information.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { CallCenterPermissionKey.CallCenter_CrisisLine, RegistrationPermissionKey.Registration_Demography, CallCenterPermissionKey.CallCenter_LawLiaison }, Permissions =new string[]{ Permission.Update, Permission.Create })]
        [HttpPut]
        public IHttpActionResult UpdateCallerInformation(CallCenterProgressNoteModel model)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationRuleEngine.UpdateCallerInformation(model), Request);
        }

        /// <summary>
        /// Updates the Modified On in caller information Table.
        /// </summary>
        /// <param name="callCenterHeaderID">The call center header identifier.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateModifiedOn(CallCenterProgressNoteModel model)
        {
            return new HttpResult<Response<CallCenterProgressNoteModel>>(_callerInformationRuleEngine.UpdateModifiedOn(model), Request);
        }

        [HttpGet]
        public IHttpActionResult GetCallCenterAssessmentResponse(long callCenterHeaderID, long assessmentID)
        {
            return new HttpResult<Response<CallCenterAssessmentResponseModel>>(_callerInformationRuleEngine.GetCallCenterAssessmentResponse(callCenterHeaderID, assessmentID), Request);
        }

        [HttpGet]
        public IHttpActionResult GetCallCenterAssessmentResponseByContactID(long contactID)
        {
            return new HttpResult<Response<CallCenterAssessmentResponseModel>>(_callerInformationRuleEngine.GetCallCenterAssessmentResponseByContactID(contactID), Request);
        }

        [HttpPost]
        public IHttpActionResult AddCallCenterAssessmentResponse(CallCenterAssessmentResponseModel model)
        {
            return new HttpResult<Response<CallCenterAssessmentResponseModel>>(_callerInformationRuleEngine.AddCallCenterAssessmentResponse(model), Request);
        }
    }
}