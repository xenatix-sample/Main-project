using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.ECI.Referrals;
using Axis.Model.Common;
using Axis.Model.ECI.Referrals;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.ECI.Referrals
{
    public class ReferralAdditionalDetailController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralAdditionalDetailDataProvider referralDataProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralAdditionalDetailController(IReferralAdditionalDetailDataProvider referralDataProvider)
        {
            this.referralDataProvider = referralDataProvider;
        }


        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralAdditionalDetail(long contactID)
        {
            return new HttpResult<Response<ReferralAdditionalDetailModel>>(referralDataProvider.GetReferralAdditionalDetail(contactID), Request);
        }

        /// <summary>
        /// Adds the referral .
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferralAdditionalDetail(ReferralAdditionalDetailModel model)
        {
            return new HttpResult<Response<ReferralAdditionalDetailModel>>(referralDataProvider.AddReferralAdditionalDetail(model), Request);
        }

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralAdditionalDetail(ReferralAdditionalDetailModel model)
        {
            return new HttpResult<Response<ReferralAdditionalDetailModel>>(referralDataProvider.UpdateReferralAdditionalDetail(model), Request);
        }
    }
}
