using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referrals;
using Axis.Service.Registration.Referral;

namespace Axis.RuleEngine.Registration.Referral
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Registration.Referral.IReferralAdditionalDetailRuleEngine" />
    public class ReferralAdditionalDetailRuleEngine : IReferralAdditionalDetailRuleEngine
    {
        /// <summary>
        /// The referral 
        /// </summary>
        private readonly IReferralAdditionalDetailService referralService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralRuleEngine" /> class.
        /// </summary>
        /// <param name="referral">The referral .</param>
        public ReferralAdditionalDetailRuleEngine(IReferralAdditionalDetailService referral)
        {
            this.referralService = referral;
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> GetReferralAdditionalDetail(long contactID)
        {
            return referralService.GetReferralAdditionalDetail(contactID);
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralDetailsModel> GetReferralsDetails(long contactID)
        {
            return referralService.GetReferralsDetails(contactID);
        }

        /// <summary>
        /// Adds the referral .
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> AddReferralAdditionalDetail(ReferralAdditionalDetailModel referral)
        {
            return referralService.AddReferralAdditionalDetail(referral);
        }

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralAdditionalDetailModel> UpdateReferralAdditionalDetail(ReferralAdditionalDetailModel referral)
        {
            return referralService.UpdateReferralAdditionalDetail(referral);
        }

        /// <summary>
        /// Delete the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<ReferralDetailsModel> DeleteReferralDetails(long contactID)
        {
            return referralService.DeleteReferralDetails(contactID);
        }
    }
}
