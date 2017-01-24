using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;

namespace Axis.Plugins.Registration.Repository.Referrals.Requestor
{
    public interface IReferralDemographicsRepository
    {
        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
        Response<ReferralDemographicsViewModel> GetReferralDemographics(long referralID);

        /// <summary>
        /// Adds the referral demographics.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        Response<ReferralDemographicsViewModel> AddReferralDemographics(ReferralDemographicsViewModel referralDemographics);

        /// <summary>
        /// Updates the referral demographics.
        /// </summary>
        /// <param name="referralDemographics">The referral demographics.</param>
        /// <returns></returns>
        Response<ReferralDemographicsViewModel> UpdateReferralDemographics(ReferralDemographicsViewModel referralDemographics);

        /// <summary>
        /// Deletes the referral demographics.
        /// </summary>
        /// <param name="referralID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralDemographicsViewModel> DeleteReferralDemographics(long referralID, DateTime modifiedOn);
    }
}