using System;
using Axis.Helpers.Validation;
using Axis.Model.Common;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Model.Clinical;
using Axis.RuleEngine.Helpers.Results;
using System.Collections.Generic;
using System.Web.Http;
using System.Web.Http.Cors;
using Axis.RuleEngine.Clinical.Vital;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Clinical
{
    /// <summary>
    ///
    /// </summary>
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    public class VitalController : ApiController
    {
        /// <summary>
        /// The contact Vital rule engine
        /// </summary>
        private readonly IVitalRuleEngine vitalRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="VitalController"/> class.
        /// </summary>
        /// <param name="vitalRuleEngine">The contact Vital rule engine.</param>
        public VitalController(IVitalRuleEngine vitalRuleEngine)
        {
            this.vitalRuleEngine = vitalRuleEngine;
        }

        /// <summary>
        /// Gets the contact Vitals.
        /// </summary>
        /// <param name="ContactId">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Vitals_Vitals, Permission = Permission.Read)]
        public IHttpActionResult GetContactVitals(long ContactId)
        {
            return new HttpResult<Response<VitalModel>>(vitalRuleEngine.GetContactVitals(ContactId), Request);
        }

        /// <summary>
        /// Adds the Vital.
        /// </summary>
        /// <param name="vital">The vital.</param>
        /// <returns></returns>
        [HttpPost]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Vitals_Vitals, Permission = Permission.Create)]
        public IHttpActionResult AddVital(VitalModel vital)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<VitalModel>>(vitalRuleEngine.AddVital(vital), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<VitalModel>() { DataItems = new List<VitalModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<VitalModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Updates the Vital.
        /// </summary>
        /// <param name="contact">The vital.</param>
        /// <returns></returns>
        [HttpPut]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Vitals_Vitals, Permission = Permission.Update)]
        public IHttpActionResult UpdateVital(VitalModel vital)
        {
            if (ModelState.IsValid)
            {
                return new HttpResult<Response<VitalModel>>(vitalRuleEngine.UpdateVital(vital), Request);
            }
            else
            {
                var errorMessage = ModelState.ParseError();
                var validationResponse = new Response<VitalModel>() { DataItems = new List<VitalModel>(), ResultCode = -1, ResultMessage = errorMessage };
                return new HttpResult<Response<VitalModel>>(validationResponse, Request);
            }
        }

        /// <summary>
        /// Deletes the Vital.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        [Authorization(PermissionKey = ClinicalPermissionKey.Clinical_Vitals_Vitals, Permission = Permission.Delete)]
        public IHttpActionResult DeleteVital(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<VitalModel>>(vitalRuleEngine.DeleteVital(id, modifiedOn), Request);
        }
    }
}
