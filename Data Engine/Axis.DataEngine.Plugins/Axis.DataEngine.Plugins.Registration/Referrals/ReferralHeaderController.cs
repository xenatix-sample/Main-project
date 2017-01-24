using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Requestor;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// Controller for referral Requestor/Requestor
    /// </summary>
    public class ReferralHeaderController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralHeaderDataProvider referralDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralHeaderController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralHeaderController(IReferralHeaderDataProvider referralDataProvider)
        {
            this.referralDataProvider = referralDataProvider;
        }


        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralHeader(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralHeaderModel>>(referralDataProvider.GetReferralHeader(referralHeaderID), Request);
        }

        /// <summary>
        /// Adds the referral Requestor Detail.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferralHeader(ReferralHeaderModel model)
        {
            return new HttpResult<Response<ReferralHeaderModel>>(referralDataProvider.AddReferralHeader(model), Request);
        }

        /// <summary>
        /// Updates the referral Requestor Detail.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralHeader(ReferralHeaderModel model)
        {
            return new HttpResult<Response<ReferralHeaderModel>>(referralDataProvider.UpdateReferralHeader(model), Request);
        }

      
    }
}