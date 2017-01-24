using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.Service.Registration.Referrals.Information;

namespace Axis.RuleEngine.Registration.Referrals.Information
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralReferredInformationRuleEngine : IReferralReferredInformationRuleEngine
    {
        /// <summary>
        /// The referral service
        /// </summary>
        private readonly IReferralReferredInformationService referralReferredInformation;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralReferredInformationRuleEngine"/> class.
        /// </summary>
        /// <param name="referralService">The referral service.</param>
        public ReferralReferredInformationRuleEngine(IReferralReferredInformationService referralReferredInformation)
        {
            this.referralReferredInformation = referralReferredInformation;
        }


        /// <summary>
        /// Gets the referral referred information
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader id.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> GetReferredInformation(long referralHeaderID)
        {
            return referralReferredInformation.GetReferredInformation(referralHeaderID);
        }

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The referral referred information.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> AddReferredInformation(ReferralReferredInformationModel referral)
        {
            return referralReferredInformation.AddReferredInformation(referral);
        }

        /// <summary>
        /// Updates the referral referred information.
        /// </summary>
        /// <param name="referral">The referred information.</param>
        /// <returns></returns>
        public Response<ReferralReferredInformationModel> UpdateReferredInformation(ReferralReferredInformationModel referral)
        {
            return referralReferredInformation.UpdateReferredInformation(referral);
        }
    }
}