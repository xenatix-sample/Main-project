using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.RuleEngine.Helpers.Controllers;
using Axis.RuleEngine.Helpers.Results;
using Axis.RuleEngine.Registration.Referrals;
using System.Threading.Tasks;
using System.Web.Http;
using Axis.Constant;
using Axis.RuleEngine.Helpers.Filters;

namespace Axis.RuleEngine.Plugins.Registration
{
    /// <summary>
    /// Controller for ReferralSearch
    /// </summary>
    public class ReferralSearchController : BaseApiController
    {
        /// <summary>
        /// The _referral search rule engine
        /// </summary>
        private readonly IReferralSearchRuleEngine _referralSearchRuleEngine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSearchController"/> class.
        /// </summary>
        /// <param name="referralSearchRuleEngine">The referral search rule engine.</param>
        public ReferralSearchController(IReferralSearchRuleEngine referralSearchRuleEngine)
        {
            _referralSearchRuleEngine = referralSearchRuleEngine;
        }

        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Referrer, Permission = Permission.Read)]
        [HttpGet]
        public IHttpActionResult GetReferrals(string searchStr, int searchType, long userID)
        {
            return new HttpResult<Response<ReferralSearchModel>>(_referralSearchRuleEngine.GetReferrals(searchStr, searchType, userID), Request);
        }

        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="reasonForDelete">The reason for delete.</param>
        /// <returns></returns>
        [Authorization(PermissionKey = ReferralsPermissionKey.Referrals_Referral_Referrer, Permission = Permission.Delete)]
        [HttpDelete]
        public IHttpActionResult DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn)
        {
            return new HttpResult<Response<ReferralSearchModel>>(_referralSearchRuleEngine.DeleteReferral(id, reasonForDelete, modifiedOn), Request);
        }
    }
}
