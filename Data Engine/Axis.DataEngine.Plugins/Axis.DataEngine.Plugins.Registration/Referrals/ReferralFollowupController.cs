using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Followup;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// Controller for referral followup/outcome
    /// </summary>
    public class ReferralFollowupController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralFollowupDataProvider referralDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralFollowupController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralFollowupController(IReferralFollowupDataProvider referralDataProvider)
        {
            this.referralDataProvider = referralDataProvider;
        }

        /// <summary>
        /// Gets the referral followups.
        /// </summary>
        /// <param name="referralHeaderID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralFollowups(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralDataProvider.GetReferralFollowups(referralHeaderID), Request);
        }

        /// <summary>
        /// Gets the referral followup.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral outcome detail identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralFollowup(long referralOutcomeDetailID)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralDataProvider.GetReferralFollowup(referralOutcomeDetailID), Request);
        }

        /// <summary>
        /// Adds the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralDataProvider.AddReferralFollowup(referral), Request);
        }

        /// <summary>
        /// Updates the referral followup.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralFollowup(ReferralOutcomeDetailsModel referral)
        {
            return new HttpResult<Response<ReferralOutcomeDetailsModel>>(referralDataProvider.UpdateReferralFollowup(referral), Request);
        }
    }
}