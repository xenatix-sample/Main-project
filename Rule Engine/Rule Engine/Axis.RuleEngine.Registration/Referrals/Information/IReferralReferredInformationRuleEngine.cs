using Axis.Model.Common;
using Axis.Model.Registration.Referral;

namespace Axis.RuleEngine.Registration.Referrals.Information
{
    /// <summary>
    /// Interface for refferal referred information
    /// </summary>
    public interface IReferralReferredInformationRuleEngine
    {
        /// <summary>
        /// Gets the Referral referred information
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader id.</param>
        /// <returns></returns>
        Response<ReferralReferredInformationModel> GetReferredInformation(long referralHeaderID);

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralReferredInformationModel> AddReferredInformation(ReferralReferredInformationModel Referral);

        /// <summary>
        /// Updates the referral referred information 
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralReferredInformationModel> UpdateReferredInformation(ReferralReferredInformationModel Referral);
        
    }
}