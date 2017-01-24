using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Information;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    /// <summary>
    /// Referral referred to information controller
    /// </summary>
    public class ReferralReferredInformationController : BaseApiController
    {

        #region Class Variables

        /// <summary>
        /// The referral Referred Information repository
        /// </summary>
        private readonly IReferralReferredInformationRepository referralReferredInformationRepository;

        #endregion Class Variables

        #region Constructors



        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupController"/> class.
        /// </summary>
        /// <param name="referralReferredInformationRepository">The referral Information repository.</param>
        public ReferralReferredInformationController(IReferralReferredInformationRepository referralReferredInformationRepository)
        {
            this.referralReferredInformationRepository = referralReferredInformationRepository;
        }

        #endregion Constructors

        #region Json Results


        /// <summary>
        /// Gets the referral referred information.
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralReferredInformationViewModel> GetReferredInformation(long referralHeaderID)
        {
            return referralReferredInformationRepository.GetReferredInformation(referralHeaderID);
        }

        /// <summary>
        /// Adds the referral referred information.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralReferredInformationViewModel> AddReferredInformation(ReferralReferredInformationViewModel referral)
        {
            return referralReferredInformationRepository.AddReferredInformation(referral);
        }

        /// <summary>
        /// Updates the referral referred information.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralReferredInformationViewModel> UpdateReferredInformation(ReferralReferredInformationViewModel referral)
        {
            return referralReferredInformationRepository.UpdateReferredInformation(referral);
        }

        #endregion Json Results
    }
}