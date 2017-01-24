using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Service.Registration.Referral
{
    public interface IReferralConcernDetailService
    {
        /// <summary>
        /// Gets the referral ConcernDetail.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralConcernDetailModel> GetReferralConcernDetail(long referralAdditionalDetailID);

        /// <summary>
        /// Adds the referral ConcernDetail.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        Response<ReferralConcernDetailModel> AddReferralConcernDetail(ReferralConcernDetailModel referralConcernDetail);

        /// <summary>
        /// Updates the referral ConcernDetail.
        /// </summary>
        /// <param name="ReferralConcernDetail">The referral ConcernDetail.</param>
        /// <returns></returns>
        Response<ReferralConcernDetailModel> UpdateReferralConcernDetail(ReferralConcernDetailModel referralConcernDetail);

        /// <summary>
        /// Delete the referral ConcernDetail.
        /// </summary>
        /// <param name="referralConcernDetailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralConcernDetailModel> DeleteReferralConcernDetail(long referralConcernDetailID, DateTime modifiedOn);

    }
}