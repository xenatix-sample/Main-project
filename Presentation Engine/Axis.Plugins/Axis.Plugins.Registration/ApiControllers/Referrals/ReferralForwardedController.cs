using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Forwarded;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The referral Forwarded repository
        /// </summary>
        private readonly IReferralForwardedRepository referralForwardedRepository;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedController" /> class.
        /// </summary>
        /// <param name="referralForwardedRepository">The referral Forwarded repository.</param>
        public ReferralForwardedController(IReferralForwardedRepository referralForwardedRepository)
        {
            this.referralForwardedRepository = referralForwardedRepository;
        }

        #endregion Constructors

        #region Json Results

        [HttpGet]
        public Response<ReferralForwardedViewModel> GetReferralForwardedDetails(long referralHeaderID)
        {
            return referralForwardedRepository.GetReferralForwardedDetails(referralHeaderID);
        }
        
        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral Forwarded detail identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralForwardedViewModel> GetReferralForwardedDetail(long referralForwardedDetailID)
        {
            return referralForwardedRepository.GetReferralForwardedDetail(referralForwardedDetailID);
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralForwardedViewModel> AddReferralForwardedDetail(ReferralForwardedViewModel referral)
        {
            return referralForwardedRepository.AddReferralForwardedDetail(referral);
        }

        /// <summary>
        /// Updates the referral Forwarded.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralForwardedViewModel> UpdateReferralForwardedDetail(ReferralForwardedViewModel referral)
        {
            return referralForwardedRepository.UpdateReferralForwardedDetail(referral);
        }

        #endregion Json Results
    }
}