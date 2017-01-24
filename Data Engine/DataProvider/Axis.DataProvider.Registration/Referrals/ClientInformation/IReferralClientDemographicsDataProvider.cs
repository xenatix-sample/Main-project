using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referral;
using Axis.Model.Registration.Referrals;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public interface IReferralClientDemographicsDataProvider
    {
        /// <summary>
        /// Gets the Referral Client Demographics
        /// </summary>
        /// <param name="ReferralAdditionalDetailID">The referral AdditionalDetail identifier.</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> GetClientDemographics(long ReferralID);

        /// <summary>
        /// Adds the Referral client demographics.
        /// </summary>
        /// <param name="referral">The referral demographics .</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> AddClientDemographics(ContactDemographicsModel referralDemographics);

        /// <summary>
        /// Updates the Referral client demographics.
        /// </summary>
        /// <param name="referral">The referral demographics .</param>
        /// <returns></returns>
        Response<ContactDemographicsModel> UpdateClientDemographics(ContactDemographicsModel referralDemographics);
    }
}
