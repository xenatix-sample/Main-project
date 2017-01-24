using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web.Http;
using Axis.Configuration;
using Axis.Constant;
using Axis.Model.Account;
using Axis.RuleEngine.Account;
using Axis.RuleEngine.Helpers.Results;
using Axis.Security;
using System.Diagnostics;
using Axis.Model.Common;
using Axis.RuleEngine.Service.Helpers;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Consents;
using Axis.Model.Consents;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Service
{

    /// <summary>
    /// 
    /// </summary>
    public class ConsentsController : BaseApiController
    {
        /// <summary>
        /// The consents rule engine
        /// </summary>
        private readonly IConsentsRuleEngine _consentsRuleEngine = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentsController" /> class.
        /// </summary>
        /// <param name="consentsRuleEngine">The consents.</param>
        public ConsentsController(IConsentsRuleEngine consentsRuleEngine)
        {
            this._consentsRuleEngine = consentsRuleEngine;
        }

        /// <summary>
        /// Gets the Consents.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ConsentsPermissionKey.Consents_Assessment_Agency, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetConsents(long contactID)
        {
            return new HttpResult<Response<ConsentsModel>>(_consentsRuleEngine.GetConsents(contactID), Request);
        }

        /// <summary>
        /// Adds the consents.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ConsentsPermissionKey.Consents_Assessment_Agency, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddConsent(ConsentsModel consent)
        {
            return new HttpResult<Response<ConsentsModel>>(_consentsRuleEngine.AddConsent(consent), Request);
        }

        /// <summary>
        /// Updates the consents.
        /// </summary>
        /// <param name="consent">The consent.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ConsentsPermissionKey.Consents_Assessment_Agency, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateConsent(ConsentsModel consent)
        {
            return new HttpResult<Response<ConsentsModel>>(_consentsRuleEngine.UpdateConsent(consent), Request);
        }

    }
}
