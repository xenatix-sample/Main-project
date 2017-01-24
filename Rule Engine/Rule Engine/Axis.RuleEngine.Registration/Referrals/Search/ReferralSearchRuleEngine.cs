using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Service.Registration.Referrals;

namespace Axis.RuleEngine.Registration.Referrals
{
    /// <summary>
    /// Rule engine for referral search
    /// </summary>
    /// <seealso cref="Axis.RuleEngine.Registration.Referrals.IReferralSearchRuleEngine" />
    public class ReferralSearchRuleEngine : IReferralSearchRuleEngine
    {
        /// <summary>
        /// The _referral search service
        /// </summary>
        private readonly IReferralSearchService _referralSearchService;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralSearchRuleEngine"/> class.
        /// </summary>
        /// <param name="referralSearchService">The referral search service.</param>
        public ReferralSearchRuleEngine(IReferralSearchService referralSearchService)
        {
            this._referralSearchService = referralSearchService;
        }

        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        public Response<ReferralSearchModel> GetReferrals(string searchStr, int searchType, long userID)
        {
            return _referralSearchService.GetReferrals(searchStr, searchType, userID);
        }

        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reasonForDelete"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralSearchModel> DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn)
        {
            return _referralSearchService.DeleteReferral(id, reasonForDelete, modifiedOn);
        }
    }
}
