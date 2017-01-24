using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Disposition;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    public class ReferralDispositionController : BaseApiController
    {
        /// <summary>
        /// The referral followup repository
        /// </summary>
        private readonly IReferralDispositionRepository referralDispositionRepository;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionController"/> class.
        /// </summary>
        /// <param name="referralFollowupRepository">The referral disposition repository.</param>
        public ReferralDispositionController(IReferralDispositionRepository referralDispositionRepository)
        {
            this.referralDispositionRepository = referralDispositionRepository;
        }

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralDispositionViewModel> GetReferralDispositionDetail(long referralHeaderID)
        {
            return referralDispositionRepository.GetReferralDispositionDetail(referralHeaderID);
        }

        /// <summary>
        /// Adds the referral disposition detail.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralDispositionViewModel> AddReferralDispositionDetail(ReferralDispositionViewModel referral)
        {
            return referralDispositionRepository.AddReferralDisposition(referral);
        }


        /// <summary>
        /// update the referral disposition detail.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralDispositionViewModel> UpdateReferralDisposition(ReferralDispositionViewModel referral)
        {
            return referralDispositionRepository.UpdateReferralDisposition(referral);
        }

    }
}
