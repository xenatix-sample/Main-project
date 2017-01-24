using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Forwarded;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedController : BaseApiController
    {
        /// <summary>
        /// The _ReferralForwarded data provider
        /// </summary>
        private readonly IReferralForwardedDataProvider _ReferralForwardedDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralForwardedController" /> class.
        /// </summary>
        /// <param name="ReferralForwardedDataProvider">The ReferralForwarded data provider.</param>
        public ReferralForwardedController(IReferralForwardedDataProvider ReferralForwardedDataProvider)
        {
            _ReferralForwardedDataProvider = ReferralForwardedDataProvider;
        }

        /// <summary>
        /// Gets the referral forwarded
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralForwardedDetails(long ReferralHeaderID)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(_ReferralForwardedDataProvider.GetReferralForwardedDetails(ReferralHeaderID), Request);
        }
        
        /// <summary>
        /// Gets the  RedferralForwarded.
        /// </summary>
        /// <param name="ReferralDetail">The ReferralDetail identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralForwardedDetail(long ReferralForwardedDetailID)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(_ReferralForwardedDataProvider.GetReferralForwardedDetail(ReferralForwardedDetailID), Request);
        }

        /// <summary>
        /// Adds the ReferralForwarded.
        /// </summary>
        /// <param name="Referral">The Referral.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferralForwardedDetail(ReferralForwardedModel Referral)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(_ReferralForwardedDataProvider.AddReferralForwardedDetail(Referral), Request);
        }

        /// <summary>
        /// Updates the RedferralForwarded.
        /// </summary>
        /// <param name="Referral>The Referral.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralForwardedDetail(ReferralForwardedModel Referral)
        {
            return new HttpResult<Response<ReferralForwardedModel>>(_ReferralForwardedDataProvider.UpdateReferralForwardedDetail(Referral), Request);
        }

       
    }
}