using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.Helpers.Validation;
using System.Web.Http;
using System.Collections.Generic;
using System;
using Axis.RuleEngine.Registration;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class SelfPayController : BaseApiController
    {
        /// <summary>
        /// The self pay rule engine
        /// </summary>
        private readonly ISelfPayRuleEngine selfPayRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="SelfPayController" /> class.
        /// </summary>
        /// <param name="selfPayRuleEngine">The self pay  rule engine.</param>
        public SelfPayController(ISelfPayRuleEngine selfPayRuleEngine)
        {
            this.selfPayRuleEngine = selfPayRuleEngine;
        }


        /// <summary>
        /// Gets the self pay list.
        /// </summary>
        /// <param name="selfPayHeaderID">The self pay identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_SelfPay_SelfPay, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetSelfPayDetails(long selfPayID)
        {
            return new HttpResult<Response<SelfPayModel>>(selfPayRuleEngine.GetSelfPayDetails(selfPayID), Request);
        }


        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_SelfPay_SelfPay, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddSelfPay(SelfPayModel selfPay)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<SelfPayModel>>(selfPayRuleEngine.AddSelfPay(selfPay), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<SelfPayModel>() { DataItems = new List<SelfPayModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<SelfPayModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Adds the self pay
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_SelfPay_SelfPay, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddSelfPayHeader(SelfPayModel selfPay)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<SelfPayModel>>(selfPayRuleEngine.AddSelfPayHeader(selfPay), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<SelfPayModel>() { DataItems = new List<SelfPayModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<SelfPayModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the self pay.
        /// </summary>
        /// <param name="selfPay">The self pay.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_SelfPay_SelfPay, Permissions = new string[] { Permission.Create, Permission.Update })]
        [HttpPut]
        public IHttpActionResult UpdateSelfPay(SelfPayModel selfPay)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<SelfPayModel>>(selfPayRuleEngine.UpdateSelfPay(selfPay), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<SelfPayModel>() { DataItems = new List<SelfPayModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<SelfPayModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// delete the self pay.
        /// </summary>
        /// <param name="contactDischargeNoteID">The self pay identifier.</param>
        /// <param name="modifiedOn">The modified on</param>
        /// <returns></returns>
        [Authorization(PermissionKey = BenefitsPermissionKey.Benefits_SelfPay_SelfPay, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteSelfPay(long selfPayID, DateTime modifiedOn)
        {
            return new HttpResult<Response<SelfPayModel>>(selfPayRuleEngine.DeleteSelfPay(selfPayID, modifiedOn), Request);
        }
    }
}
