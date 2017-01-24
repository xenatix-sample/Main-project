using Axis.Plugins.Registration.Models.Referrals;
using Axis.Plugins.Registration.Repository.Referrals.Followup;
using Axis.PresentationEngine.Helpers.Controllers;
using System.Web.Http;
using Axis.Model.Common;

namespace Axis.Plugins.Registration.ApiControllers.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralFollowupController : BaseApiController
    {
        #region Class Variables

        /// <summary>
        /// The referral followup repository
        /// </summary>
        private readonly IReferralFollowupRepository referralFollowupRepository;

        #endregion Class Variables

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupController" /> class.
        /// </summary>
        /// <param name="referralFollowupRepository">The referral followup repository.</param>
        public ReferralFollowupController(IReferralFollowupRepository referralFollowupRepository)
        {
            this.referralFollowupRepository = referralFollowupRepository;
        }

        #endregion Constructors

        #region Json Results

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralOutcomeDetailsViewModel> GetReferralFollowups(long referralHeaderID)
        {
            return referralFollowupRepository.GetReferralFollowups(referralHeaderID);
        }

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public Response<ReferralOutcomeDetailsViewModel> GetReferralFollowup(long referralOutcomeDetailID)
        {
            return referralFollowupRepository.GetReferralFollowup(referralOutcomeDetailID);
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPost]
        public Response<ReferralOutcomeDetailsViewModel> AddReferralFollowup(ReferralOutcomeDetailsViewModel referral)
        {
            return referralFollowupRepository.AddReferralFollowup(referral);
        }

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPut]
        public Response<ReferralOutcomeDetailsViewModel> UpdateReferralFollowup(ReferralOutcomeDetailsViewModel referral)
        {
            return referralFollowupRepository.UpdateReferralFollowup(referral);
        }

        #endregion Json Results
    }
}