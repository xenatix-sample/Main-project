using System;
using System.Threading.Tasks;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    /// Referral repository
    /// </summary>
    public interface IReferralRepository
    {
        /// <summary>
        /// Gets referrals
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        Response<ReferralModel> GetReferrals(long contactId);

        /// <summary>
        /// Get referrals aynchronously
        /// </summary>
        /// <param name="contactId"></param>
        /// <returns></returns>
        Task<Response<ReferralModel>> GetReferralsAsync(long contactId);

        /// <summary>
        /// Adds referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        Response<ReferralViewModel> AddReferral(ReferralViewModel referral);

        /// <summary>
        /// Updates referral
        /// </summary>
        /// <param name="referral">referral model</param>
        /// <returns></returns>
        Response<ReferralViewModel> UpdateReferral(ReferralViewModel referral);

        /// <summary>
        /// Deletes referral
        /// </summary>
        /// <param name="id">Referral Id</param>
        /// <returns></returns>
        Response<ReferralViewModel> DeleteReferral(long id, DateTime modifiedOn);

        /// <summary>
        /// Updates referral contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        Response<ReferralContactViewModel> UpdateReferralContact(ReferralContactViewModel referralContact);

        /// <summary>
        /// Deletes referral contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <returns></returns>
        Response<ReferralContactViewModel> DeleteReferalContact(long referralContactId, DateTime modifiedOn);
    }
}