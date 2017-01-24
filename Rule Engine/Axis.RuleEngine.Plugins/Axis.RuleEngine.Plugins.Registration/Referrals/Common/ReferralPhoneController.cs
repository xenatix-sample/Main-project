using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.Common;
using System.Collections.Generic;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration.Referrals.Common
{
    public class ReferralPhoneController : BaseApiController
    {
        /// <summary>
        /// The referral phone rule engine
        /// </summary>
        private readonly IReferralPhoneRuleEngine referralPhoneRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralPhoneController(IReferralPhoneRuleEngine referralDataProvider)
        {
            this.referralPhoneRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = GeneralPermissionKey.General_General_Phone, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetPhones(long referralID, int contactTypeID)
        {
            return new HttpResult<Response<ReferralPhoneModel>>(referralPhoneRuleEngine.GetPhones(referralID, contactTypeID), Request);
        }

        /// <summary>
        /// Adds the update phones.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = GeneralPermissionKey.General_General_Phone, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddUpdatePhones(List<ReferralPhoneModel> referral)
        {
            return new HttpResult<Response<ReferralPhoneModel>>(referralPhoneRuleEngine.AddUpdatePhones(referral), Request);
        }

        /// <summary>
        /// Deletes the referral phone.
        /// </summary>
        /// <param name="referralPhoneID">The referral phone identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = GeneralPermissionKey.General_General_Phone, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteReferralPhone(long referralPhoneID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralPhoneModel>>(referralPhoneRuleEngine.DeleteReferralPhone(referralPhoneID, modifiedOn), Request);
        }
    }
}