using System;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.DataProvider.Registration
{
    /// <summary>
    /// Interface for Referral data provider
    /// </summary>
    public interface IReferralDataProvider
    {
        /// <summary>
        /// Gets referrals
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        Response<ReferralModel> GetReferrals(long contactId);

        /// <summary>
        /// Adds referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        Response<ReferralModel> AddReferral(ReferralModel referral);

        /// <summary>
        /// Updates referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        Response<ReferralModel> UpdateReferral(ReferralModel referral);

        /// <summary>
        /// Deletes referral
        /// </summary>
        /// <param name="id">Referral Id</param>
        /// <returns></returns>
        Response<ReferralModel> DeleteReferral(long id);

        /// <summary>
        /// Updates referral contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        Response<ReferralContactModel> UpdateReferralContact(ReferralContactModel referralContact);

        /// <summary>
        /// Deletes referral contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralContactModel> DeleteReferalContact(long referralContactId, DateTime modifiedOn);
    }
}