using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.Referrals;
using Axis.Model.Common;
using Axis.Model.ECI.Referrals;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.ECI.Referrals
{
    public class ReferralConcernDetailController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralConcernDetailDataProvider referralDataProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralConcernDetailController(IReferralConcernDetailDataProvider referralDataProvider)
        {
            this.referralDataProvider = referralDataProvider;
        }


        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralConcernDetail(long referralAdditionalDetailID)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralDataProvider.GetReferralConcernDetail(referralAdditionalDetailID), Request);
        }

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferralConcernDetail(ReferralConcernDetailModel model)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralDataProvider.AddReferralConcernDetail(model), Request);
        }

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralConcernDetail(ReferralConcernDetailModel model)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralDataProvider.UpdateReferralConcernDetail(model), Request);
        }


        /// <summary>
        /// Delete the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteReferralConcernDetail(long referralConcernDetailID)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralDataProvider.DeleteReferralConcernDetail(referralConcernDetailID), Request);
        }
    }
}
