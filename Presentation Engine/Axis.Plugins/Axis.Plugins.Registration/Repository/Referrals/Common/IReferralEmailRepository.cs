using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Repository.Referrals.Common
{
    public interface IReferralEmailRepository
    {
        /// <summary>
        /// Gets the Emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        Response<ReferralEmailViewModel> GetEmails(long referralID, int contactTypeID);

        /// <summary>
        /// Adds and update Emails.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralEmailViewModel> AddUpdateEmails(List<ReferralEmailViewModel> referral);

        /// <summary>
        /// Deletes the Email.
        /// </summary>
        /// <param name="referralEmailID">The referral Email identifier.</param>
        /// <returns></returns>
        Response<ReferralEmailViewModel> DeleteEmail(long referralEmailID, DateTime modifiedOn);
    }
}