using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.Plugins.Registration.Repository.Referrals.Requestor
{
    public interface IReferralHeaderRepository
    {
        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralHeaderViewModel> GetReferralHeader(long referralID);

        /// <summary>
        /// Adds the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        Response<ReferralHeaderViewModel> AddReferralHeader(ReferralHeaderViewModel referralHeader);

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        Response<ReferralHeaderViewModel> UpdateReferralHeader(ReferralHeaderViewModel referralHeader);

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralHeaderViewModel> DeleteReferralHeader(long referralID, DateTime modifiedOn);
    }
}