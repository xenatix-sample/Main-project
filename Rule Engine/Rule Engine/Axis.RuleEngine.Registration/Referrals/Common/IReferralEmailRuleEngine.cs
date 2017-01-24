using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration.Referrals.Common
{
    public interface IReferralEmailRuleEngine
    {
        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        Response<ReferralEmailModel> GetEmails(long referralID, int contactTypeID);



        /// <summary>
        /// Adds and update emails.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralEmailModel> AddUpdateEmails(List<ReferralEmailModel> referral);


        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="referralEmailID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralEmailModel> DeleteEmail(long referralEmailID, DateTime modifiedOn);
    }
}