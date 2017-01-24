using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.Service.Registration.Referrals.Forwarded
{
    /// <summary>
    /// Interface for refferal service
    /// </summary>
    public interface IReferralForwardedService
    {
        /// <summary>
        /// Gets the referral forwarded
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> GetReferralForwardedDetails(long ReferralHeaderID);

        /// <summary>
        /// Gets the referral Forwarded
        /// </summary>
        /// <param name="referralForwardedDetailID">The referral forwarded detail identifier.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> GetReferralForwardedDetail(long referralForwardedDetailID);

        /// <summary>
        /// Adds the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> AddReferralForwardedDetail(ReferralForwardedModel referral);

        /// <summary>
        /// Updates the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> UpdateReferralForwardedDetail(ReferralForwardedModel referral);


        
    }
}