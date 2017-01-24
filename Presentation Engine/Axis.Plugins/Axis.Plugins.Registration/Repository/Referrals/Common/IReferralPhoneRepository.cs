using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Repository.Referrals.Common
{
    public interface IReferralPhoneRepository
    {
        /// <summary>
        /// Gets the Phonees.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        Response<ReferralPhoneViewModel> GetPhones(long referralID, int contactTypeID);

        /// <summary>
        /// Adds and update Phonees.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralPhoneViewModel> AddUpdatePhones(List<ReferralPhoneViewModel> referral);

        /// <summary>
        /// Deletes the Phone.
        /// </summary>
        /// <param name="referralPhoneID">The referral Phone identifier.</param>
        /// <returns></returns>
        Response<ReferralPhoneViewModel> DeleteReferralPhone(long referralPhoneID, DateTime modifiedOn);
    }
}