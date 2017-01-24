using Axis.Constant;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Filters;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals.Disposition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;

namespace Axis.RuleEngine.Plugins.Registration.Referrals
{
    public class ReferralDispositionController : BaseApiController
    {
        #region Class Variables
        /// <summary>
        /// The referral Disposition data provider
        /// </summary>
        readonly IReferralDispositionRuleEngine referralDispositionRuleEngine;

        #endregion

        #region Constructors
        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionController"/> class.
        /// </summary>
        /// <param name="referralDispositionDataProvider">The referral disposition data provider.</param>
        public ReferralDispositionController(IReferralDispositionRuleEngine referralDispositionDataProvider)
        {
            this.referralDispositionRuleEngine = referralDispositionDataProvider;
        }
        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral disposition detail identifier.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Disposition, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferralDispositionDetail(long referralHeaderID)
        {
            return new HttpResult<Response<ReferralDispositionModel>>(referralDispositionRuleEngine.GetReferralDispositionDetail(referralHeaderID), Request);
        }
        /// <summary>
        /// Adds the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Disposition, Permission = Permission.Create)]
        [HttpPost]
        public IHttpActionResult AddReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            return new HttpResult<Response<ReferralDispositionModel>>(referralDispositionRuleEngine.AddReferralDisposition(referralDisposition), Request);
        }

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Disposition, Permission = Permission.Update)]
        [HttpPut]
        public IHttpActionResult UpdateReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            return new HttpResult<Response<ReferralDispositionModel>>(referralDispositionRuleEngine.UpdateReferralDisposition(referralDisposition), Request);
        }
        #endregion

    }
}
