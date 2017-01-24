using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.Forwarded;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// Controller for referral forwarded
    /// </summary>
    public class ReferralForwardedController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralForwardedRuleEngine referralForwardedRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralForwardedController(IReferralForwardedRuleEngine referralForwardedRuleEngine)
        {
            this.referralForwardedRuleEngine = referralForwardedRuleEngine;
        }

        /// <summary>
        /// Gets the referral forwarded.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_ForwardedTo, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralForwardedDetails(long ReferralHeaderID)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(referralForwardedRuleEngine.GetReferralForwardedDetails(ReferralHeaderID), Request);
        }


        /// <summary>
        /// Gets the referral forwarded.
        /// </summary>
        /// <param name="referralID">The referral detail identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_ForwardedTo, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralForwardedDetail(long ReferralForwardedDetailID)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(referralForwardedRuleEngine.GetReferralForwardedDetail(ReferralForwardedDetailID), Request);
        }


        /// <summary>
        /// Adds the referral forawrded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_ForwardedTo, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddReferralForwardedDetail(ReferralForwardedModel referral)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(referralForwardedRuleEngine.AddReferralForwardedDetail(referral), Request);
        }

        /// <summary>
        /// Updates the referral forawrded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_ForwardedTo, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferralForwardedDetail(ReferralForwardedModel referral)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(referralForwardedRuleEngine.UpdateReferralForwardedDetail(referral), Request);
        }
    }
}