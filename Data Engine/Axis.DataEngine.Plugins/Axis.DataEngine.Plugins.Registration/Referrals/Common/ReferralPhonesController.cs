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
     
    public class ReferralPhonesController : BaseApiController
    {

        /// <summary>
        /// The referral phone data provider
        /// </summary>
        private readonly IReferralPhoneDataProvider referralPhoneDataProvider;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhonesController"/> class.
        /// </summary>
        /// <param name="referralPhoneDataProvider">The referral phone data provider.</param>
        public ReferralPhonesController(IReferralPhoneDataProvider referralPhoneDataProvider)
        {
            this.referralPhoneDataProvider = referralPhoneDataProvider;
        }


        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [HttpGet]
        public IHttpActionResult GetPhones(long referralID, int? contactTypeID)
        {
            return new HttpResult<Response<ReferralPhoneModel>>(referralPhoneDataProvider.GetPhones(referralID, contactTypeID), Request);
        }


        /// <summary>
        /// Adds the update phones.
        /// </summary>
        /// <param name="referralPhoneModel">The referral phone model.</param>
        /// <returns></returns>
        [HttpPost]
        public IHttpActionResult AddUpdatePhones(List<ReferralPhoneModel> referralPhoneModel)
        {


                return new HttpResult<Response<ReferralPhoneModel>>(referralPhoneDataProvider.AddUpdatePhones(referralPhoneModel), Request);

        }


        /// <summary>
        /// Deletes the phone.
        /// </summary>
        /// <param name="referralPhoneId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [HttpDelete]
        public IHttpActionResult DeletePhone(long referralPhoneId, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralPhoneModel>>(referralPhoneDataProvider.DeleteReferralPhone(referralPhoneId, modifiedOn), Request);
        }
    }
}
