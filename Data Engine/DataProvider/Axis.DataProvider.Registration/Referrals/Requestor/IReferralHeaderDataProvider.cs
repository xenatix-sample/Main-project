﻿using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;

namespace Axis.DataProvider.Registration.Referrals.Requestor
{
    public interface IReferralHeaderDataProvider
    {

        /// <summary>
        /// Gets the referral header.
        /// </summary>
        /// <param name="referralHeaderID">The referral header identifier.</param>
        /// <returns></returns>
        Response<ReferralHeaderModel> GetReferralHeader(long referralHeaderID);

        /// <summary>
        /// Adds the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        Response<ReferralHeaderModel> AddReferralHeader(ReferralHeaderModel referralHeader);

        /// <summary>
        /// Updates the referral header.
        /// </summary>
        /// <param name="referralHeader">The referral header.</param>
        /// <returns></returns>
        Response<ReferralHeaderModel> UpdateReferralHeader(ReferralHeaderModel referralHeader);

        /// <summary>
        /// Deletes the referral header.
        /// </summary>
        /// <param name="referralHeaderID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralHeaderModel> DeleteReferralHeader(long referralHeaderID, DateTime modifiedOn);

    }
}