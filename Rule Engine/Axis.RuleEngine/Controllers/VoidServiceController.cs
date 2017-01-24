using System;
using Axis.Model.Common;
using Axis.Model.RecordedServices;
using Axis.RuleEngine.RecordedServices.VoidService;
using Axis.Service.RecordedServices.VoidService;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Service.Controllers
{
    /// <summary>
    ///
    /// </summary>
    public class VoidServiceController : BaseApiController
    {
        /// <summary>
        /// The voidService engine
        /// </summary>
        private readonly IVoidServiceRuleEngine voidServiceRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="VoidServiceController"/> class.
        /// </summary>
        /// <param name="voidServiceRuleEngine">The voidService engine.</param>
        public VoidServiceController(IVoidServiceRuleEngine voidServiceRuleEngine)
        {
            this.voidServiceRuleEngine = voidServiceRuleEngine;
        }

        /// <summary>
        /// Adds the VoidService.
        /// </summary>
        /// <param name="voidService">The voidService.</param>
        /// <returns></returns>
        [Authorization(Modules = new String[] { Module.Benefits, Module.Intake, Module.LawLiaison }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult VoidRecordedService(VoidServiceModel voidService)
        {
            return new HttpResult<Response<VoidServiceModel>>(voidServiceRuleEngine.VoidService(voidService), Request);
        }

        /// <summary>
        /// To set call center service as void
        /// </summary>
        /// <param name="voidService">voidService</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new String[] { CallCenterPermissionKey.CallCenter_CrisisLine }, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult VoidServiceCallCenter(VoidServiceModel voidService)
        {
            return new HttpResult<Response<VoidServiceModel>>(voidServiceRuleEngine.VoidServiceCallCenter(voidService), Request);
        }

        /// <summary>
        /// Get the VoidService.
        /// </summary>
        /// <param name="serviceRecordingID">The serviceRecordingID.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetVoidService(int serviceRecordingID)
        {
            return new HttpResult<Response<VoidServiceModel>>(voidServiceRuleEngine.GetVoidService(serviceRecordingID), Request);
        }

    }
}