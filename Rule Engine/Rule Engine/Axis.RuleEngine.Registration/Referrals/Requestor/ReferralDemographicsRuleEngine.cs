using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Service.Registration.Referrals.Requestor;

namespace Axis.RuleEngine.Registration.Referrals.Requestor
{
    public class ReferralDemographicsRuleEngine : IReferralDemographicsRuleEngine
    {
        /// <summary>
        /// The referral demographics
        /// </summary>
        private readonly IReferralDemographicsService referralDemographics;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsRuleEngine"/> class.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        public ReferralDemographicsRuleEngine(IReferralDemographicsService referralDemographics)
        {
            this.referralDemographics = referralDemographics;
        }

        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> GetReferralDemographics(long referralID)
        {
            return referralDemographics.GetReferralDemographics(referralID);
        }

        /// <summary>
        /// Adds the referral demographics.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> AddReferralDemographics(ReferralDemographicsModel referral)
        {
            return referralDemographics.AddReferralDemographics(referral);
        }

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> UpdateReferralDemographics(ReferralDemographicsModel referral)
        {
            return referralDemographics.UpdateReferralDemographics(referral);
        }

        /// <summary>
        /// Deletes the referral demographics.
        /// </summary>
        /// <param name="referralID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralDemographicsModel> DeleteReferralDemographics(long referralID, DateTime modifiedOn)
        {
            return referralDemographics.DeleteReferralDemographics(referralID, modifiedOn);
        }
    }
}