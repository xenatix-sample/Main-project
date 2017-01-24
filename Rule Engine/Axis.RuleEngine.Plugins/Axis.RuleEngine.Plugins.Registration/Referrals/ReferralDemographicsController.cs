using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.Requestor;
using System.Web.Http;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Plugins.Registration.Referrals
{
    public class ReferralDemographicsController : BaseApiController
    {
        /// <summary>
        /// The referral demographics rule engine
        /// </summary>
        private readonly IReferralDemographicsRuleEngine referralDemographicsRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDemographicsController"/> class.
        /// </summary>
        /// <param name="referralDataProvider">The referral data provider.</param>
        public ReferralDemographicsController(IReferralDemographicsRuleEngine referralDataProvider)
        {
            this.referralDemographicsRuleEngine = referralDataProvider;
        }

        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
      
        [HttpGet]
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Referrer, Permission = Permission.Read)]
        public IHttpActionResult GetReferralDemographics(long referralID)
        {
            return new HttpResult<Response<ReferralDemographicsModel>>(referralDemographicsRuleEngine.GetReferralDemographics(referralID), Request);
        }

        /// <summary>
        /// Adds the referral demographics.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
     
        [HttpPost]
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Referrer, Permission = Permission.Create)]
        public IHttpActionResult AddReferralDemographics(ReferralDemographicsModel referral)
        {
            return new HttpResult<Response<ReferralDemographicsModel>>(referralDemographicsRuleEngine.AddReferralDemographics(referral), Request);
        }

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
      
        [HttpPut]
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Referrer, Permission = Permission.Update)]
        public IHttpActionResult UpdateReferralDemographics(ReferralDemographicsModel referral)
        {
            return new HttpResult<Response<ReferralDemographicsModel>>(referralDemographicsRuleEngine.UpdateReferralDemographics(referral), Request);
        }

        /// <summary>
        /// Deletes the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
    
        [HttpDelete]
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Referrer, Permission = Permission.Delete)]
        public IHttpActionResult DeleteReferralDemographics(long referralID, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralDemographicsModel>>(referralDemographicsRuleEngine.DeleteReferralDemographics(referralID, modifiedOn), Request);
        }
    }
}