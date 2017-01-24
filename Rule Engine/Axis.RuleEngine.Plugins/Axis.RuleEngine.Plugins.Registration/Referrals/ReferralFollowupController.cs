using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.Followup;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// Controller for referral followup/outcome
    /// </summary>
    public class ReferralFollowupController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralFollowupRuleEngine referralFollowupRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralFollowupController(IReferralFollowupRuleEngine referralDataProvider)
        {
            this.referralFollowupRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_FollowUpOutcome, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralFollowups(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralFollowupRuleEngine.GetReferralFollowups(referralHeaderID), Request);
        }

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_FollowUpOutcome, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralFollowup(long referralOutcomeDetailID)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralFollowupRuleEngine.GetReferralFollowup(referralOutcomeDetailID), Request);
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_FollowUpOutcome, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralFollowupRuleEngine.AddReferralFollowup(referral), Request);
        }

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_FollowUpOutcome, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralFollowupRuleEngine.UpdateReferralFollowup(referral), Request);
        }
    }
}