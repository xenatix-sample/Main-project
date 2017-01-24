using Axis.Model.Common;
using Axis.Model.ECI.Referrals;

namespace Axis.DataProvider.ECI.Referrals
{
    public interface IReferralAdditionalDetailDataProvider
    {

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralAdditionalDetailModel> GetReferralAdditionalDetail(long contactID);


       /// <summary>
       /// Adds the referral .
       /// </summary>
       /// <param name="referral">The referral .</param>
       /// <returns></returns>
        Response<ReferralAdditionalDetailModel> AddReferralAdditionalDetail(ReferralAdditionalDetailModel referral);


       /// <summary>
       /// Updates the referral .
       /// </summary>
       /// <param name="referral">The referral .</param>
       /// <returns></returns>
        Response<ReferralAdditionalDetailModel> UpdateReferralAdditionalDetail(ReferralAdditionalDetailModel referral);


    }
}
