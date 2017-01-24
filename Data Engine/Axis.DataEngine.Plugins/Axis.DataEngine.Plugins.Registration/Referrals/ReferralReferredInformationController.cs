using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Forwarded;
using Axis.DataProvider.Registration.Referrals.Information;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// Referral referred to information controller
    /// </summary>
    public class ReferralReferredInformationController : BaseApiController
    {
        /// <summary>
        /// The _Referral Referred information data provider
        /// </summary>
        private readonly IReferralReferredInformationDataProvider _IReferralReferredInformationDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralReferredInformationController" /> class.
        /// </summary>
        /// <param name="ReferralReferredInformationDataProvider">The Referral referred information data provider.</param>
        public ReferralReferredInformationController(IReferralReferredInformationDataProvider ReferralReferredInformationDataProvider)
        {
            _IReferralReferredInformationDataProvider = ReferralReferredInformationDataProvider;
        }

        /// <summary>
        /// Gets the  Referral referred information 
        /// </summary>
        /// <param name="ReferredToInformationID">The referralHeader identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferredInformation(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralReferredInformationModel>>(_IReferralReferredInformationDataProvider.GetReferredInformation(referralHeaderID), Request);
        }

        /// <summary>
        /// Adds the Referral referred information.
        /// </summary>
        /// <param name="Referral">The Referral referred information.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferredInformation(ReferralReferredInformationModel Referral)
        {
            return new HttpResult<Response<ReferralReferredInformationModel>>(_IReferralReferredInformationDataProvider.AddReferredInformation(Referral), Request);
        }

        /// <summary>
        /// Updates the Referral referred information.
        /// </summary>
        /// <param name="Referral>The Referral referred information.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferredInformation(ReferralReferredInformationModel Referral)
        {
            return new HttpResult<Response<ReferralReferredInformationModel>>(_IReferralReferredInformationDataProvider.UpdateReferredInformation(Referral), Request);
        }

       
    }
}