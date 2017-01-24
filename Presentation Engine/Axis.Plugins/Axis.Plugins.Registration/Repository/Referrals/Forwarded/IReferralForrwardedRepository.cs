using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.Plugins.Registration.Repository.Referrals.Forwarded
{
    /// <summary>
    /// Referral repository
    /// </summary>
    public interface IReferralForwardedRepository
    {
        /// <summary>
        /// Gets the referral forwarded.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralForwardedViewModel> GetReferralForwardedDetails(long referralHeaderID);

        /// <summary>
        /// Gets the referral Forwarded
        /// </summary>
        /// <param name="referralForwardedDetailID">The referral Forwarded detail identifier.</param>
        /// <returns></returns>
        Response<ReferralForwardedViewModel> GetReferralForwardedDetail(long referralForwardedDetailID);

        /// <summary>
        /// Adds the referral Forwarded
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedViewModel> AddReferralForwardedDetail(ReferralForwardedViewModel referral);

        /// <summary>
        /// Updates the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedViewModel> UpdateReferralForwardedDetail(ReferralForwardedViewModel referral);
               
    }
}