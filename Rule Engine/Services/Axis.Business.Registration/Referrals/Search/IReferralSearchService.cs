using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals;

namespace Axis.Service.Registration.Referrals
{
    /// <summary>
    /// Interface for referral service
    /// </summary>
    public interface IReferralSearchService
    {
        /// <summary>
        /// Gets the referrals.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
        Response<ReferralSearchModel> GetReferrals(string searchStr, int searchType, long userID);
        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reasonForDelete"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralSearchModel> DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn);
    }
}
