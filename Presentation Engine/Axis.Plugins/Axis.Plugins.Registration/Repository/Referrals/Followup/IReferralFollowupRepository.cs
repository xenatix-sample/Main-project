using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.Plugins.Registration.Repository.Referrals.Followup
{
    /// <summary>
    /// Referral repository
    /// </summary>
    public interface IReferralFollowupRepository
    {
        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsViewModel> GetReferralFollowups(long referralHeaderID);

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsViewModel> GetReferralFollowup(long referralOutcomeDetailID);

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsViewModel> AddReferralFollowup(ReferralOutcomeDetailsViewModel referral);

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsViewModel> UpdateReferralFollowup(ReferralOutcomeDetailsViewModel referral);
    }
}