using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referral;

namespace Axis.DataProvider.Registration.Referrals.Requestor
{
   public interface IReferralDemographicsDataProvider
    {

        /// <summary>
        /// Gets the referral demographics.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <returns></returns>
       Response<ReferralDemographicsModel> GetReferralDemographics(long referralID);


       /// <summary>
       /// Adds the referral demographics.
       /// </summary>
       /// <param name="referralDemographics">The referral demographics.</param>
       /// <returns></returns>
       Response<ReferralDemographicsModel> AddReferralDemographics(ReferralDemographicsModel referralDemographics);


       /// <summary>
       /// Updates the referral demographics.
       /// </summary>
       /// <param name="referralDemographics">The referral demographics.</param>
       /// <returns></returns>
       Response<ReferralDemographicsModel> UpdateReferralDemographics(ReferralDemographicsModel referralDemographics);


       /// <summary>
       /// Deletes the referral demographics.
       /// </summary>
       /// <param name="referralID"></param>
       /// <param name="modifiedOn"></param>
       /// <returns></returns>
       Response<ReferralDemographicsModel> DeleteReferralDemographics(long referralID, DateTime modifiedOn);
    }
}
