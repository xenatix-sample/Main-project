using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Repository.Referral
{
    public interface IReferralConcernDetailRepository
    {
        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralConcernDetailViewModel> GetReferralConcernDetail(long referralAdditionalDetailID);

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        Response<ReferralConcernDetailViewModel> AddReferralConcernDetail(ReferralConcernDetailViewModel referralConcernDetail);

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        Response<ReferralConcernDetailViewModel> UpdateReferralConcernDetail(ReferralConcernDetailViewModel referralConcernDetail);

        /// <summary>
        /// Deletes the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralConcernDetailViewModel> DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn);
    }
}