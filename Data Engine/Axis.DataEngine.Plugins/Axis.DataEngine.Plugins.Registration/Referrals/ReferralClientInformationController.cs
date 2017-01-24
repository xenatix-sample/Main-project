using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataProvider.Registration.Referrals.ClientInformation;
using Axis.DataEngine.Helpers.Results;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    public class ReferralClientInformationController : BaseApiController
    {
        /// <summary>
        /// The Client Information data provider
        /// </summary>
        private readonly IReferralClientInformationDataProvider referralClientInformationDataProvider;

         /// <summary>
        /// Initializes a new instance of the <see cref="ReferralClientInformationController"/> class.
        /// </summary>
        /// <param name="ReferralClientInformationDataProvider">The referral client information data provider.</param>
        public ReferralClientInformationController(IReferralClientInformationDataProvider ReferralClientInformationDataProvider)
        {
            this.referralClientInformationDataProvider = ReferralClientInformationDataProvider;
        }

        /// <summary>
        /// Gets the referral client information.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetClientInformation(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralClientInformationModel>>(referralClientInformationDataProvider.GetClientInformation(referralHeaderID), Request);
        }

        /// <summary>
        /// Adds the referral client information.
        /// </summary>
        /// <param name="referralClientInformation">The referral client.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddClientInformation(ReferralClientInformationModel referralClientInformation)
        {
            return new HttpResult<Response<ReferralClientInformationModel>>(referralClientInformationDataProvider.AddClientInformation(referralClientInformation), Request);
        }

        /// <summary>
        /// Updates the referral client information.
        /// </summary>
        /// <param name="referralClientInformation">The referral client.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateClientInformation(ReferralClientInformationModel referralClientInformation)
        {
            return new HttpResult<Response<ReferralClientInformationModel>>(referralClientInformationDataProvider.UpdateClientInformation(referralClientInformation), Request);
        }
    }
}
