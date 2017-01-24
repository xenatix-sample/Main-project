using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration.Referral;

namespace Axis.RuleEngine.Registration.Referral
{
    public class ReferralConcernDetailRuleEngine : IReferralConcernDetailRuleEngine
    {
        /// <summary>
        /// The referral ConcernDetail
        /// </summary>
        private readonly IReferralConcernDetailService referralConcernDetail;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailRuleEngine"/> class.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        public ReferralConcernDetailRuleEngine(IReferralConcernDetailService referralConcernDetail)
        {
            this.referralConcernDetail = referralConcernDetail;
        }

        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> GetReferralConcernDetail(long referralID)
        {
            return referralConcernDetail.GetReferralConcernDetail(referralID);
        }

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> AddReferralConcernDetail(ReferralConcernDetailModel referral)
        {
            return referralConcernDetail.AddReferralConcernDetail(referral);
        }

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> UpdateReferralConcernDetail(ReferralConcernDetailModel referral)
        {
            return referralConcernDetail.UpdateReferralConcernDetail(referral);
        }
        /// <summary>
        /// Delete the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralConcernDetailModel> DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn)
        {
            return referralConcernDetail.DeleteReferralConcernDetail(referralConcernDetailID, modifiedOn);
        }
    }
}