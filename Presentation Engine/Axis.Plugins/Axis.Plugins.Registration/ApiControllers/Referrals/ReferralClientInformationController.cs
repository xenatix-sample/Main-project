using Axis.PresentationEngine.Helpers.Controllers;
using Axis.Plugins.Registration.Repository.Referrals.ClientInformation;
using Axis.Model.Registration.Referrals;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    public class ReferralClientInformationController : BaseApiController
    {
        private readonly IReferralClientInformationRepository referralClientRepository;

        public ReferralClientInformationController(IReferralClientInformationRepository referralClientRepository)
        {
            this.referralClientRepository = referralClientRepository;
        }

        /// <summary>
        /// Gets the referral client information.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralClientInformationModel> GetClientInformation(long referralHeaderID)
        {
            return referralClientRepository.GetClientInformation(referralHeaderID);
        }

        /// <summary>
        /// Adds the referral client information.
        /// </summary>
        /// <param name="referral">The referral client information.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralClientInformationModel> AddClientInformation(ReferralClientInformationModel referral)
        {
            return referralClientRepository.AddClientInformation(referral);
        }

        /// <summary>
        /// Update the referral client information.
        /// </summary>
        /// <param name="referral">The referral client information.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralClientInformationModel> UpdateClientInformation(ReferralClientInformationModel referral)
        {
            return referralClientRepository.UpdateClientInformation(referral);
        }


    }
}
