using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Model.Registration.Referrals;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referral;
using System.Web.Http;

/// <summary>
/// 
/// </summary>
namespace Axis.RuleEngine.Plugins.Registration
{
    public class ReferralAdditionalDetailController : BaseApiController
    {
        /// <summary>
        /// The referral  rule engine
        /// </summary>
        private readonly IReferralAdditionalDetailRuleEngine referralRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralAdditionalDetailController(IReferralAdditionalDetailRuleEngine referralDataProvider)
        {
            this.referralRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralAdditionalDetail(long contactID)
        {
            return new HttpResult<Response<ReferralAdditionalDetailModel>>(referralRuleEngine.GetReferralAdditionalDetail(contactID), Request);
        }

        /// <summary>
        /// Gets the referral .
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralsDetails(long contactID)
        {
            return new HttpResult<Response<ReferralDetailsModel>>(referralRuleEngine.GetReferralsDetails(contactID), Request);
        }

        /// <summary>
        /// Adds the referral .
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddReferralAdditionalDetail(ReferralAdditionalDetailModel referral)
        {
            return new HttpResult<Response<ReferralAdditionalDetailModel>>(referralRuleEngine.AddReferralAdditionalDetail(referral), Request);
        }

        /// <summary>
        /// Updates the referral .
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferralAdditionalDetail(ReferralAdditionalDetailModel referral)
        {
            return new HttpResult<Response<ReferralAdditionalDetailModel>>(referralRuleEngine.UpdateReferralAdditionalDetail(referral), Request);
        }

        /// <summary>
        /// Delete Referral
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteReferralDetails(long contactID)
        {
            return new HttpResult<Response<ReferralDetailsModel>>(referralRuleEngine.DeleteReferralDetails(contactID), Request);
        }
    }
}
