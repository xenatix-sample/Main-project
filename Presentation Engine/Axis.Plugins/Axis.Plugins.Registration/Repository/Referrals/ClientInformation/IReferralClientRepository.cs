using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.Plugins.Registration.Repository.Referrals.ClientInformation
{
    /// <summary>
    /// Referral Client Repository
    /// </summary>
    public interface IReferralClientInformationRepository
    {
        /// <summary>
        /// Gets the referral client information.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralClientInformationModel> GetClientInformation(long referralHeaderID);

        /// <summary>
        /// Adds the referral client information
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralClientInformationModel> AddClientInformation(ReferralClientInformationModel referralClient);

        /// <summary>
        /// Updates the referral client information 
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralClientInformationModel> UpdateClientInformation(ReferralClientInformationModel referral);


    }
}
