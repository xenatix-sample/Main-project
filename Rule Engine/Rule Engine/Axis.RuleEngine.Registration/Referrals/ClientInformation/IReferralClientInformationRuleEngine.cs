using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.RuleEngine.Registration.Referrals.ClientInformation
{
    /// <summary>
    /// Interface for refferal client information
    /// </summary>
    public interface IReferralClientInformationRuleEngine
    {
        /// <summary>
        /// Gets the Referral Client information
        /// </summary>
        /// <param name="ReferredToInformationID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralClientInformationModel> GetClientInformation(long ReferralID);

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="ReferralClientInformationModel">The Referral.</param>
        /// <returns></returns>
        Response<ReferralClientInformationModel> AddClientInformation(ReferralClientInformationModel clientInformation);

        /// <summary>
        /// Updates the referral referred information 
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralClientInformationModel> UpdateClientInformation(ReferralClientInformationModel clientInformation);
      
    }
}
