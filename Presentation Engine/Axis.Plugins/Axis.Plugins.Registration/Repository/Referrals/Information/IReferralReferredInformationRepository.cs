using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.Plugins.Registration.Repository.Referrals.Information
{
    /// <summary>
    /// Interface for refferal referred information
    /// </summary>
    public interface IReferralReferredInformationRepository
    {
        /// <summary>
        /// Gets the Referral referred information
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader identifier.</param>
        /// <returns></returns>
        Response<ReferralReferredInformationViewModel> GetReferredInformation(long referralHeaderID);

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralReferredInformationViewModel> AddReferredInformation(ReferralReferredInformationViewModel Referral);

        /// <summary>
        /// Updates the referral referred information 
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        Response<ReferralReferredInformationViewModel> UpdateReferredInformation(ReferralReferredInformationViewModel Referral);
       
    }
}