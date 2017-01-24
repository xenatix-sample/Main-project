using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Service.Registration.Referrals.ClientInformation;

namespace Axis.RuleEngine.Registration.Referrals.ClientInformation
{
    public class ReferralClientInformationRuleEngine : IReferralClientInformationRuleEngine
    {
        /// <summary>
        /// The referral service
        /// </summary>
        private readonly IReferralClientInformationService referralService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedRuleEngine"/> class.
        /// </summary>
        /// <param name="referralService">The referral service.</param>
        public ReferralClientInformationRuleEngine(IReferralClientInformationService referralService)
        {
            this.referralService = referralService;
        }

        /// <summary>
        /// Gets the Referral Client information
        /// </summary>
        /// <param name="ReferredToInformationID">The Detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralClientInformationModel> GetClientInformation(long ReferralID)
        {
            return referralService.GetClientInformation(ReferralID);
        }

        /// <summary>
        /// Adds the referral referred information
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        public Response<ReferralClientInformationModel> AddClientInformation(ReferralClientInformationModel clientInformation)
        {
            return referralService.AddClientInformation(clientInformation);
        }

        /// <summary>
        /// Updates the referral referred information 
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        public Response<ReferralClientInformationModel> UpdateClientInformation(ReferralClientInformationModel clientInformation)
        {
            return referralService.UpdateClientInformation(clientInformation);
        }
    }
}
