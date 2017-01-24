using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Service.Registration.Referrals.Forwarded;

namespace Axis.RuleEngine.Registration.Referrals.Forwarded
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedRuleEngine : IReferralForwardedRuleEngine
    {
        /// <summary>
        /// The referral service
        /// </summary>
        private readonly IReferralForwardedService referralService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedRuleEngine"/> class.
        /// </summary>
        /// <param name="referralService">The referral service.</param>
        public ReferralForwardedRuleEngine(IReferralForwardedService referralService)
        {
            this.referralService = referralService;
        }

        /// <summary>
        /// Gets the referral forwarded
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> GetReferralForwardedDetails(long ReferralHeaderID)
        {
            return referralService.GetReferralForwardedDetails(ReferralHeaderID);
        }

        /// <summary>
        /// Gets the referral Forwarded.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral forwarded detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> GetReferralForwardedDetail(long ReferralForwardedDetailID)
        {
            return referralService.GetReferralForwardedDetail(ReferralForwardedDetailID);
        }

        /// <summary>
        /// Adds the referralForwarded
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> AddReferralForwardedDetail(ReferralForwardedModel referral)
        {
            return referralService.AddReferralForwardedDetail(referral);
        }

        /// <summary>
        /// Updates the referral Forwarded
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralForwardedModel> UpdateReferralForwardedDetail(ReferralForwardedModel referral)
        {
            return referralService.UpdateReferralForwardedDetail(referral);
        }

    }
}