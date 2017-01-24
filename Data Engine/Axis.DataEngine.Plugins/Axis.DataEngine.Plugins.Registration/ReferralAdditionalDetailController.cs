using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referral;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referrals;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.DataEngine.Helpers.Controllers.BaseApiController" />
    public class ReferralAdditionalDetailController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralAdditionalDetailDataProvider referralDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralController" /> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralAdditionalDetailController(IReferralAdditionalDetailDataProvider referralDataProvider)
        {
            this.referralDataProvider = referralDataProvider;
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralAdditionalDetail(long contactID)
        {
            return new HttpResult<Response<ReferralAdditionalDetailModel>>(referralDataProvider.GetReferralAdditionalDetail(contactID), Request);
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralsDetails(long contactID)
        {
            return new HttpResult<Response<ReferralDetailsModel>>(referralDataProvider.GetReferralsDetails(contactID), Request);
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

        /// <summary>
        /// Deletes the referral details.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteReferralDetails(long contactID)
        {
            return new HttpResult<Response<ReferralDetailsModel>>(referralDataProvider.DeleteReferralDetails(contactID), Request);
        }
    }
    }
