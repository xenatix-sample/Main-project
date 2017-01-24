using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referral;
using System.Web.Http;
using Axis.RuleEngine.Helpers.Filters;
using Axis.Constant;

namespace Axis.RuleEngine.Plugins.Registration
{
    public class ReferralConcernDetailController : BaseApiController
    {

        /// <summary>
        /// The referral concern detail rule engine
        /// </summary>
        private readonly IReferralConcernDetailRuleEngine referralConcernDetailRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralConcernDetailController" /> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralConcernDetailController(IReferralConcernDetailRuleEngine referralDataProvider)
        {
            this.referralConcernDetailRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralAdditionalDetailID">The referral additional detail identifier.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralConcernDetail(long referralAdditionalDetailID)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralConcernDetailRuleEngine.GetReferralConcernDetail(referralAdditionalDetailID), Request);
        }

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permissions = new String[] { Permission.Create, Permission.Update })]
        [HttpPost]
        public IHttpActionResult AddReferralConcernDetail(ReferralConcernDetailModel referral)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralConcernDetailRuleEngine.AddReferralConcernDetail(referral), Request);
        }

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns>IHttpActionResult.</returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferralConcernDetail(ReferralConcernDetailModel referral)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralConcernDetailRuleEngine.UpdateReferralConcernDetail(referral), Request);
        }

        /// <summary>
        /// Delete the referral ConcernDetail.
        /// </summary>
        /// <param name="referralConcernDetailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        [Authorization(PermissionKeys = new string[] { RegistrationPermissionKey.Registration_Registration_Referral, ECIPermissionKey.ECI_Registration_Referral, GeneralPermissionKey.General_General_Referral }, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralConcernDetailModel>>(referralConcernDetailRuleEngine.DeleteReferralConcernDetail(referralConcernDetailID, modifiedOn), Request);
        }

    }
}