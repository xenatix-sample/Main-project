using System;
using System.Web.Http;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for referral
    /// </summary>
    public class ReferralController : BaseApiController
    {
        private readonly IReferralRuleEngine referralRuleEngine;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="referralRuleEngine"></param>
        public ReferralController(IReferralRuleEngine referralRuleEngine)
        {
            this.referralRuleEngine = referralRuleEngine;
        }

        /// <summary>
        /// Get refferals
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Registration_Referral, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferrals(long contactId)
        {
            return new HttpResult<Response<ReferralModel>>(referralRuleEngine.GetReferrals(contactId), Request);
        }

        /// <summary>
        /// Add referral
        /// </summary>
        /// <param name="referral">Referral model</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Registration_Referral, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddReferral(ReferralModel referral)
        {
            return new HttpResult<Response<ReferralModel>>(referralRuleEngine.AddReferral(referral), Request);
        }

        /// <summary>
        /// Update referral
        /// </summary>
        /// <param name="referral">Referral model</param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Registration_Referral, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferral(ReferralModel referral)
        {
            return new HttpResult<Response<ReferralModel>>(referralRuleEngine.UpdateReferral(referral), Request);
        }

        /// <summary>
        /// Deletes referral
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Registration_Referral, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteReferral(long id, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralModel>>(referralRuleEngine.DeleteReferral(id, modifiedOn), Request);
        }

        /// <summary>
        /// Updates referral contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Registration_Referral, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferralContact(ReferralContactModel referralContact)
        {
            return new HttpResult<Response<ReferralContactModel>>(referralRuleEngine.UpdateReferralContact(referralContact), Request);
        }

        /// <summary>
        /// Deletes referral contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKey = RegistrationPermissionKey.Registration_Registration_Referral, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteReferalContact(long referralContactId, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralContactModel>>(referralRuleEngine.DeleteReferalContact(referralContactId, modifiedOn), Request);
        }
    }
}