using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.DataProvider.Registration.Referrals.Forwarded
{
    /// <summary>
    ///
    /// </summary>
    public interface IReferralForwardedDataProvider
    {
        /// <summary>
        /// Gets the referral forwarded.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> GetReferralForwardedDetails(long ReferralHeaderID);

        /// <summary>
        /// Gets the Referral Forwarded to
        /// </summary>
        /// <param name="ReferralForwardedDetailID">The Detail identifier.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> GetReferralForwardedDetail(long ReferralForwardedDetailID);

        /// <summary>
        /// Adds the referral forwarded to
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> AddReferralForwardedDetail(ReferralForwardedModel Referral);

        /// <summary>
        /// Updates the referral forwarded to
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> UpdateReferralForwardedDetail(ReferralForwardedModel Referral);

       
    }
}