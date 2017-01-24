using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.ClientInformation;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Helpers.Controllers.BaseApiController" />
    public class ReferralClientInformationController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralClientInformationRuleEngine referralClientInformationRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralClientInformationController" /> class.
        /// </summary>
        /// <param name="referralClientInformationRuleEngine">The referral client information rule engine.</param>
        public ReferralClientInformationController(IReferralClientInformationRuleEngine referralClientInformationRuleEngine)
        {
            this.referralClientInformationRuleEngine = referralClientInformationRuleEngine;
        }

        /// <summary>
        /// Gets the referral client information.
        /// </summary>
        /// <param name="ReferralID">The referral identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral, ReferralsPermissionKey.Referrals_Referral_Contact }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetClientInformation(long ReferralID)
        {
            return new HttpResult<Response<ReferralClientInformationModel>>(referralClientInformationRuleEngine.GetClientInformation(ReferralID), Request);
        }

        /// <summary>
        /// Adds the referral client information.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Contact, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddClientInformation(ReferralClientInformationModel referral)
        {
            return new HttpResult<Response<ReferralClientInformationModel>>(referralClientInformationRuleEngine.AddClientInformation(referral), Request);
        }

        /// <summary>
        /// Updates the referral client information.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Contact, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateClientInformation(ReferralClientInformationModel referral)
        {
            return new HttpResult<Response<ReferralClientInformationModel>>(referralClientInformationRuleEngine.UpdateClientInformation(referral), Request);
        }
    }
}
