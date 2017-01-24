using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public interface IReferralClientInformationDataProvider
    {
        /// <summary>
        /// Gets the Referral Client information
        /// </summary>
        /// <param name="ReferredToInformationID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralClientInformationModel> GetClientInformation(long ReferralHeaderID);

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The Referral.</param>
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
