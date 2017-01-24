using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Service.Registration.Referrals.Common;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration.Referrals.Common
{
    public class ReferralPhoneRuleEngine : IReferralPhoneRuleEngine
    {
        /// <summary>
        /// The referral phone
        /// </summary>
        private readonly IReferralPhoneService referralPhone;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralPhoneRuleEngine"/> class.
        /// </summary>
        /// <param name="referralPhone">The referral phone.</param>
        public ReferralPhoneRuleEngine(IReferralPhoneService referralPhone)
        {
            this.referralPhone = referralPhone;
        }

        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> GetPhones(long referralID, int contactTypeID)
        {
            return referralPhone.GetPhones(referralID, contactTypeID);
        }

        /// <summary>
        /// Add and update phones.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> AddUpdatePhones(List<ReferralPhoneModel> referral)
        {
            return referralPhone.AddUpdatePhones(referral);
        }

        /// <summary>
        /// Deletes the referral phone.
        /// </summary>
        /// <param name="referralID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralPhoneModel> DeleteReferralPhone(long referralID, DateTime modifiedOn)
        {
            return referralPhone.DeleteReferralPhone(referralID, modifiedOn);
        }
    }
}