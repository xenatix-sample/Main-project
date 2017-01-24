using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.Information;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration.Referrals
{
    /// <summary>
    /// Controller for referral Referred Information controller
    /// </summary>
    public class ReferralReferredInformationController : BaseApiController
    {
        /// <summary>
        /// The referral data provider
        /// </summary>
        private readonly IReferralReferredInformationRuleEngine referralReferredInformationRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralReferredInformationController"/> class.
        /// </summary>
        /// <param name="referralReferredInformationRuleEngine">The referral referred information engine</param>
        public ReferralReferredInformationController(IReferralReferredInformationRuleEngine referralReferredInformationRuleEngine)
        {
            this.referralReferredInformationRuleEngine = referralReferredInformationRuleEngine;
        }


        /// <summary>
        /// Gets the referral referred information.
        /// </summary>
        /// <param name="referralHeaderID">The referralHeader id.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_ReferredTo, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferredInformation(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralReferredInformationModel>>(referralReferredInformationRuleEngine.GetReferredInformation(referralHeaderID), Request);
        }

        /// <summary>
        /// Adds the referral referred information.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_ReferredTo, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddReferredInformation(ReferralReferredInformationModel referral)
        {
            return new HttpResult<Response<ReferralReferredInformationModel>>(referralReferredInformationRuleEngine.AddReferredInformation(referral), Request);
        }

        /// <summary>
        /// Updates the referral referred information.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_ReferredTo, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferredInformation(ReferralReferredInformationModel referral)
        {
            return new HttpResult<Response<ReferralReferredInformationModel>>(referralReferredInformationRuleEngine.UpdateReferredInformation(referral), Request);
        }
    }
}