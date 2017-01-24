using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.Requestor;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration.Referrals
{
    public class ReferralHeaderController : BaseApiController
    {
        /// <summary>
        /// The referral header rule engine
        /// </summary>
        private readonly IReferralHeaderRuleEngine referralHeaderRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralHeaderController(IReferralHeaderRuleEngine referralDataProvider)
        {
            this.referralHeaderRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { ReferralsPermissionKey.Referrals_Referral_Referrer,  RegistrationPermissionKey.Registration_Registration_Referral,ECIPermissionKey.ECI_Registration_Referral,GeneralPermissionKey.General_General_Referral }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralHeader(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralHeaderModel>>(referralHeaderRuleEngine.GetReferralHeader(referralHeaderID), Request);
        }

        /// <summary>
        /// Adds the referral header.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { ReferralsPermissionKey.Referrals_Referral_Referrer, RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddReferralHeader(ReferralHeaderModel referral)
        {
            return new HttpResult<Response<ReferralHeaderModel>>(referralHeaderRuleEngine.AddReferralHeader(referral), Request);
        }

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { ReferralsPermissionKey.Referrals_Referral_Referrer, RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferralHeader(ReferralHeaderModel referral)
        {
            return new HttpResult<Response<ReferralHeaderModel>>(referralHeaderRuleEngine.UpdateReferralHeader(referral), Request);
        }

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { ReferralsPermissionKey.Referrals_Referral_Referrer, RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteReferralHeader(long referralHeaderID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralHeaderModel>>(referralHeaderRuleEngine.DeleteReferralHeader(referralHeaderID, modifiedOn), Request);
        }
    }
}