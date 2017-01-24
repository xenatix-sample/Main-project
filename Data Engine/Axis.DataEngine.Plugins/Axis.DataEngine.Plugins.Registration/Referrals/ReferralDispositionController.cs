using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Disposition;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals
{
    public class ReferralDispositionController : BaseApiController
    {
        /// <summary>
        /// The referral disposition data provider
        /// </summary>
        private readonly IReferralDispositionDataProvider referralDispositionDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral disposition data provider.</param>
        public ReferralDispositionController(IReferralDispositionDataProvider referralDispositionDataProvider)
        {
            this.referralDispositionDataProvider = referralDispositionDataProvider;
        }

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral header identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferralDispositionDetail(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralDispositionModel>>(referralDispositionDataProvider.GetReferralDispositionDetail(referralHeaderID), Request);
        }

        /// <summary>
        /// Adds the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            return new HttpResult<Response<ReferralDispositionModel>>(referralDispositionDataProvider.AddReferralDisposition(referralDisposition), Request);
        }

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            return new HttpResult<Response<ReferralDispositionModel>>(referralDispositionDataProvider.UpdateReferralDisposition(referralDisposition), Request);
        }
    }
}
