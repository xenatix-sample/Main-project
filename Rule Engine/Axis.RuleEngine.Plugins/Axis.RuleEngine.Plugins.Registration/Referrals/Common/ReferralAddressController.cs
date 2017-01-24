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
    public class ReferralAddressController : BaseApiController
    {
        /// <summary>
        /// The referral address rule engine
        /// </summary>
        private readonly IReferralAddressRuleEngine referralAddressRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralAddressController(IReferralAddressRuleEngine referralDataProvider)
        {
            this.referralAddressRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { GeneralPermissionKey.General_General_Address, BusinessAdministrationPermissionKey.BusinessAdministration_ClientMerge_ClientMerge }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetAddresses(long referralID, int contactTypeID)
        {
            return new HttpResult<Response<ReferralAddressModel>>(referralAddressRuleEngine.GetAddresses(referralID, contactTypeID), Request);
        }

        /// <summary>
        /// Adds the update addresses.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = GeneralPermissionKey.General_General_Address, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddUpdateAddresses(List<ReferralAddressModel> referral)
        {
            return new HttpResult<Response<ReferralAddressModel>>(referralAddressRuleEngine.AddUpdateAddresses(referral), Request);
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralAddressID">The referral address identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = GeneralPermissionKey.General_General_Address, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteAddress(long referralAddressID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralAddressModel>>(referralAddressRuleEngine.DeleteAddress(referralAddressID, modifiedOn), Request);
        }
    }
}