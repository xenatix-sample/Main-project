using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Service.Registration.Referrals.Followup;

namespace Axis.RuleEngine.Registration.Referrals.Followup
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralFollowupRuleEngine : IReferralFollowupRuleEngine
    {
        /// <summary>
        /// The referral service
        /// </summary>
        private readonly IReferralFollowupService referralService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupRuleEngine"/> class.
        /// </summary>
        /// <param name="referralService">The referral service.</param>
        public ReferralFollowupRuleEngine(IReferralFollowupService referralService)
        {
            this.referralService = referralService;
        }

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> GetReferralFollowups(long referralHeaderID)
        {
            return referralService.GetReferralFollowups(referralHeaderID);
        }

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> GetReferralFollowup(long referralOutcomeDetailID)
        {
            return referralService.GetReferralFollowups(referralOutcomeDetailID);
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> AddReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            return referralService.AddReferralFollowup(referral);
        }

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralOutcomeDetailsModel> UpdateReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            return referralService.UpdateReferralFollowup(referral);
        }
    }
}