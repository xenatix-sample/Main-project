using Axis.Model.Common;
using Axis.Model.Registration.Referral;

namespace Axis.DataProvider.Registration.Referrals.Followup
{
    /// <summary>
    /// Referral followup/outcome contract
    /// </summary>
    public interface IReferralFollowupDataProvider
    {
        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsModel> GetReferralFollowups(long referralHeaderID);

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsModel> GetReferralFollowup(long referralOutcomeDetailID);

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsModel> AddReferralFollowup(ReferralOutcomeDetailsModel referral);

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralOutcomeDetailsModel> UpdateReferralFollowup(ReferralOutcomeDetailsModel referral);
    }
}