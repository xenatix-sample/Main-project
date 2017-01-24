using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.RuleEngine.Registration.Referrals.Forwarded
{
    /// <summary>
    /// Interface for refferal rule engine
    /// </summary>
    public interface IReferralForwardedRuleEngine
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
        /// <param name="referralForwardDetailID">The referral ForwardDetail identifier.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> GetReferralForwardedDetail(long referralForwardedDetailID);

        /// <summary>
        /// Adds the referral forwarded
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> AddReferralForwardedDetail(ReferralForwardedModel referral);

        /// <summary>
        /// Updates the referral forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralForwardedModel> UpdateReferralForwardedDetail(ReferralForwardedModel referral);

        
      
    }
}