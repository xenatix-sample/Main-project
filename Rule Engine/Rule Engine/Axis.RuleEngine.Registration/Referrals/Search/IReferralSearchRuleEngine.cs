using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.RuleEngine.Registration.Referrals
{
    /// <summary>
    /// Interface for Referral search
    /// </summary>
    public interface IReferralSearchRuleEngine
    {
        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        Response<ReferralSearchModel> GetReferrals(string searchStr, int searchType, long userID);
        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="reasonForDelete">The reason for delete.</param>
        /// <returns></returns>
        Response<ReferralSearchModel> DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn);
    }
}
