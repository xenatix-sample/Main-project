using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Requestor;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    public class ReferralDemographicsController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralDemographicsDataProvider referralDataProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralDemographicsController(IReferralDemographicsDataProvider referralDataProvider)
        {
            this.referralDataProvider = referralDataProvider;
        }


        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralDemographics(long referralID)
        {
            return new HttpResult<Response<ReferralDemographicsModel>>(referralDataProvider.GetReferralDemographics(referralID), Request);
        }

        /// <summary>
        /// Adds the referral demographics.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferralDemographics(ReferralDemographicsModel model)
        {
            return new HttpResult<Response<ReferralDemographicsModel>>(referralDataProvider.AddReferralDemographics(model), Request);
        }

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="model">The model.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralDemographics(ReferralDemographicsModel model)
        {
            return new HttpResult<Response<ReferralDemographicsModel>>(referralDataProvider.UpdateReferralDemographics(model), Request);
        }
    }
}
