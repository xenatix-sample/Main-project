using Axis.Constant;
using Axis.Model.CallCenter;
using Axis.Model.Common;
using Axis.RuleEngine.Common.ServiceRecording;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using System;
using System.Web.Http;
using System.Web.Http.Cors;

namespace Axis.RuleEngine.Plugins.CallCenter
{

    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Helpers.Controllers.BaseApiController" />
    public class ServiceRecordingController : BaseApiController
    {

        /// <summary>
        /// The service recording rule engine
        /// </summary>
        private readonly IServiceRecordingRuleEngine _serviceRecordingRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServiceRecordingController" /> class.
        /// </summary>
        /// <param name="serviceRecordingRuleEngine">The service recording rule engine.</param>
        public ServiceRecordingController(IServiceRecordingRuleEngine serviceRecordingRuleEngine)
        {
            _serviceRecordingRuleEngine = serviceRecordingRuleEngine;
        }

        /// <summary>
        /// Gets the Service Recording.
        /// </summary>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <param name="ContactID">The contact identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <returns></returns>
        [Authorization(Modules = new String[] { Module.Benefits, Module.Consents, Module.CrisisLine, Module.General, Module.Intake, Module.LawLiaison, Module.SiteAdministration }, PermissionKeys = new string[] { ChartPermissionKey.Chart_Chart_RecordedServices }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetServiceRecordings(int ServiceRecordingSourceID, long ContactID, DateTime? startDate=null, DateTime? endDate=null)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingRuleEngine.GetServiceRecordings(ServiceRecordingSourceID, ContactID, startDate, endDate), Request);
        }

        /// <summary>
        /// Gets the Service Recording.
        /// </summary>
        /// <param name="SourceHeaderID">The source header identifier.</param>
        /// <param name="ServiceRecordingSourceID">The service recording source identifier.</param>
        /// <returns></returns>
        [Authorization(Modules = new String[] { Module.Benefits, Module.Consents, Module.CrisisLine, Module.General, Module.Intake, Module.LawLiaison, Module.SiteAdministration }, PermissionKeys = new string[] { ChartPermissionKey.Chart_Chart_Assessment, ChartPermissionKey.Chart_Chart_RecordedServices }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetServiceRecording(long SourceHeaderID, int ServiceRecordingSourceID)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingRuleEngine.GetServiceRecording(SourceHeaderID, ServiceRecordingSourceID), Request);
        }


        /// <summary>
        /// Adds the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials, BenefitsPermissionKey.Benefits_BAPN_BAPN, IntakeFormsPermissionKey.Intake_IDD_Forms, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddServiceRecording(ServiceRecordingModel model)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingRuleEngine.AddServiceRecording(model), Request);
        }


        /// <summary>
        /// Updates the Service Recording.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine, SiteAdministrationPermissionKey.SiteAdministration_StaffManagement_Credentials, BenefitsPermissionKey.Benefits_BAPN_BAPN, IntakeFormsPermissionKey.Intake_IDD_Forms, CallCenterPermissionKey.CallCenter_LawLiaison }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateServiceRecording(ServiceRecordingModel model)
        {
            return new HttpResult<Response<ServiceRecordingModel>>(_serviceRecordingRuleEngine.UpdateServiceRecording(model), Request);
        }

        [HttpGet]
        public IHttpActionResult GetProgramUnits(string datakey)
        {
            return new HttpResult<Response<ProgramUnitModel>>(_serviceRecordingRuleEngine.GetProgramUnits(datakey), Request);
        }
    }
}
