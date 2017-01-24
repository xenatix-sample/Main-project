using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Service.Registration.Referrals.Common;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration.Referrals.Common
{
    public class ReferralEmailRuleEngine : IReferralEmailRuleEngine
    {
        /// <summary>
        /// The referral email
        /// </summary>
        private readonly IReferralEmailService referralEmail;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralEmailRuleEngine"/> class.
        /// </summary>
        /// <param name="referralEmail">The referral email.</param>
        public ReferralEmailRuleEngine(IReferralEmailService referralEmail)
        {
            this.referralEmail = referralEmail;
        }

        /// <summary>
        /// Gets the emails.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralEmailModel> GetEmails(long referralID, int contactTypeID)
        {
            return referralEmail.GetEmails(referralID, contactTypeID);
        }

        /// <summary>
        /// Adds and update emails.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralEmailModel> AddUpdateEmails(List<ReferralEmailModel> referral)
        {
            return referralEmail.AddUpdateEmails(referral);
        }

        /// <summary>
        /// Deletes the email.
        /// </summary>
        /// <param name="referralID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralEmailModel> DeleteEmail(long referralID, DateTime modifiedOn)
        {
            return referralEmail.DeleteEmail(referralID, modifiedOn);
        }
    }
}