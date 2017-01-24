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
    public class ReferralEmailController : BaseApiController
    {
        /// <summary>
        /// The referral email rule engine
        /// </summary>
        private readonly IReferralEmailRuleEngine referralEmailRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralEmailController(IReferralEmailRuleEngine referralDataProvider)
        {
            this.referralEmailRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_Email, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetEmails(long referralID, int contactTypeID)
        {
            return new HttpResult<Response<ReferralEmailModel>>(referralEmailRuleEngine.GetEmails(referralID, contactTypeID), Request);
        }

        /// <summary>
        /// Adds the update emails.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = GeneralPermissionKey.General_General_Email, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddUpdateEmails(List<ReferralEmailModel> referral)
        {
            return new HttpResult<Response<ReferralEmailModel>>(referralEmailRuleEngine.AddUpdateEmails(referral), Request);
        }

        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="referralEmailID">The referral email identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = GeneralPermissionKey.General_General_Email, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteEmail(long referralEmailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralEmailModel>>(referralEmailRuleEngine.DeleteEmail(referralEmailID, modifiedOn), Request);
        }
    }
}