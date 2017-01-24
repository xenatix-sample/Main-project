using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referrals;

namespace Axis.RuleEngine.Registration.Referral
{
    /// <summary>
    /// 
    /// </summary>
    public interface IReferralAdditionalDetailRuleEngine
    {
        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReferralAdditionalDetailModel> GetReferralAdditionalDetail(long contactID);

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReferralDetailsModel> GetReferralsDetails(long contactID);

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

        /// <summary>
        /// Delete the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        Response<ReferralDetailsModel> DeleteReferralDetails(long contactID);
    }
}
