using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Service.Registration.Referrals.Requestor;

namespace Axis.RuleEngine.Registration.Referrals.Requestor
{
    public class ReferralHeaderRuleEngine : IReferralHeaderRuleEngine
    {
        /// <summary>
        /// The referral header
        /// </summary>
        private readonly IReferralHeaderService referralHeader;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderRuleEngine"/> class.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        public ReferralHeaderRuleEngine(IReferralHeaderService referralHeader)
        {
            this.referralHeader = referralHeader;
        }

        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> GetReferralHeader(long referralHeaderID)
        {
            return referralHeader.GetReferralHeader(referralHeaderID);
        }

        /// <summary>
        /// Adds the referral header.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> AddReferralHeader(ReferralHeaderModel referral)
        {
            return referralHeader.AddReferralHeader(referral);
        }

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> UpdateReferralHeader(ReferralHeaderModel referral)
        {
            return referralHeader.UpdateReferralHeader(referral);
        }

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralHeaderID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralHeaderModel> DeleteReferralHeader(long referralHeaderID, DateTime modifiedOn)
        {
            return referralHeader.DeleteReferralHeader(referralHeaderID, modifiedOn);
        }
    }
}