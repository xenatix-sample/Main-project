using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository.Referrals
{
    public interface IReferralSearchRepository
    {
        /// <summary>
        /// Gets the referrals asynchronous.
        /// </summary>
        /// <param name="searchStr">The search string.</param>
        /// <returns></returns>
        Task<Response<ReferralSearchViewModel>> GetReferralsAsync(string searchStr, int searchType, long userID);
        /// <summary>
        /// Deletes the referral.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="reasonForDelete"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralSearchViewModel> DeleteReferral(long id, string reasonForDelete, DateTime modifiedOn);
    }
}
