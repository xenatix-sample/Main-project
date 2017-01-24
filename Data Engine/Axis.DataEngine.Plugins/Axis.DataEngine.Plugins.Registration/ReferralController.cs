using System;
using System.Web.Http;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for referral
    /// </summary>
    public class ReferralController : BaseApiController
    {
        private readonly IReferralDataProvider referralDataProvider;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="referralDataProvider"></param>
        public ReferralController(IReferralDataProvider referralDataProvider)
        {
            this.referralDataProvider = referralDataProvider;
        }

        /// <summary>
        /// Get refferals
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetReferrals(long contactId)
        {
            return new HttpResult<Response<ReferralModel>>(referralDataProvider.GetReferrals(contactId), Request);
        }

        /// <summary>
        /// Add referral
        /// </summary>
        /// <param name="referral">Referral model</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddReferral(ReferralModel referral)
        {
            return new HttpResult<Response<ReferralModel>>(referralDataProvider.AddReferral(referral), Request);
        }

        /// <summary>
        /// Update referral
        /// </summary>
        /// <param name="referral">Referral model</param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferral(ReferralModel referral)
        {
            return new HttpResult<Response<ReferralModel>>(referralDataProvider.UpdateReferral(referral), Request);
        }

        /// <summary>
        /// Delete referral
        /// </summary>
        /// <param name="id">Referral Id</param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteReferral(long id)
        {
            return new HttpResult<Response<ReferralModel>>(referralDataProvider.DeleteReferral(id), Request);
        }

        /// <summary>
        /// Update referral contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        [HttpPut]
        public IHttpActionResult UpdateReferralContact(ReferralContactModel referralContact)
        {
            return new HttpResult<Response<ReferralContactModel>>(referralDataProvider.UpdateReferralContact(referralContact), Request);
        }

        /// <summary>
        /// Delete referal contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteReferalContact(long referralContactId, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralContactModel>>(referralDataProvider.DeleteReferalContact(referralContactId, modifiedOn), Request);
        }
    }
}