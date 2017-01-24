using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public interface IReferralClientAdditionalDetailsDataProvider
    {
        /// <summary>
        /// Gets the Referral Client AdditionalDetail
        /// </summary>
        /// <param name="ReferralAdditionalDetailID">The referral AdditionalDetail identifier.</param>
        /// <returns></returns>
        Response<ReferralClientAdditionalDetailsModel> GetClientAdditionalDetail(long ReferralHeaderID);

        /// <summary>
        /// Adds the client AdditionalDetail.
        /// </summary>
        /// <param name="referral">The referral AdditionalDetail .</param>
        /// <returns></returns>
        Response<ReferralClientAdditionalDetailsModel> AddClientAdditionalDetail(ReferralClientAdditionalDetailsModel referralAdditionalDetails);

        /// <summary>
        /// Updates the client AdditionalDetail.
        /// </summary>
        /// <param name="referral">The referral AdditionalDetail .</param>
        /// <returns></returns>
        Response<ReferralClientAdditionalDetailsModel> UpdateClientAdditionalDetail(ReferralClientAdditionalDetailsModel referralAdditionalDetails);
       
    }
}
