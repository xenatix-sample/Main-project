using System;
using Axis.DataEngine.Helpers.Controllers;
using Axis.DataEngine.Helpers.Results;
using Axis.DataProvider.Registration.Referrals.Common;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;
using System.Web.Http;

namespace Axis.DataEngine.Plugins.Registration.Referrals.Common
{


    public class ReferralEmailController : BaseApiController
    {

        /// <summary>
        /// The _email data provider
        /// </summary>
        readonly IReferralEmailDataProvider emailDataProvider;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailController"/> class.
        /// </summary>
        /// <param name="emailDataProvider">The email data provider.</param>
        public ReferralEmailController(IReferralEmailDataProvider emailDataProvider)
        {
            this.emailDataProvider = emailDataProvider;
        }

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetEmails(long referralID, int? contactTypeID)
        {
            return new HttpResult<Response<ReferralEmailModel>>(emailDataProvider.GetEmails(referralID, contactTypeID), Request);
        }


        /// <summary>
        /// Adds and update emails.
        /// </summary>
        /// <param name="email">The contact emails.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUpdateEmails(List<ReferralEmailModel> email)
        {
            return new HttpResult<Response<ReferralEmailModel>>(emailDataProvider.AddUpdateEmails(email), Request);
        }



        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="referralEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeleteEmail(long referralEmailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralEmailModel>>(emailDataProvider.DeleteEmail(referralEmailID, modifiedOn), Request);
        }

    }
}
