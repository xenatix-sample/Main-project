using System;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Service.Registration;

namespace Axis.RuleEngine.Registration
{
    public class ReferralRuleEngine : IReferralRuleEngine
    {
        private readonly IReferralService referralService;

        /// <summary>
        /// constructor
        /// </summary>
        /// <param name="referralService"></param>
        public ReferralRuleEngine(IReferralService referralService)
        {
            this.referralService = referralService;
        }

        /// <summary>
        /// Gets referrals
        /// </summary>
        /// <param name="contactId">Contact Id</param>
        /// <returns></returns>
        public Response<ReferralModel> GetReferrals(long contactId)
        {
            return referralService.GetReferrals(contactId);
        }

        /// <summary>
        /// Adds referral
        /// </summary>
        /// <param name="referral"></param>
        /// <returns></returns>
        public Response<ReferralModel> AddReferral(ReferralModel referral)
        {
            return referralService.AddReferral(referral);
        }

        /// <summary>
        /// Updates referral
        /// </summary>
        /// <param name="referral"></param>
        /// <returns></returns>
        public Response<ReferralModel> UpdateReferral(ReferralModel referral)
        {
            return referralService.UpdateReferral(referral);
        }

        /// <summary>
        /// Deletes referral
        /// </summary>
        /// <param name="id"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralModel> DeleteReferral(long id, DateTime modifiedOn)
        {
            return referralService.DeleteReferral(id, modifiedOn);
        }

        /// <summary>
        /// Updates referral contact
        /// </summary>
        /// <param name="referralContact"></param>
        /// <returns></returns>
        public Response<ReferralContactModel> UpdateReferralContact(ReferralContactModel referralContact)
        {
            return referralService.UpdateReferralContact(referralContact);
        }

        /// <summary>
        /// Deletes referral contact
        /// </summary>
        /// <param name="referralContactId"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralContactModel> DeleteReferalContact(long referralContactId, DateTime modifiedOn)
        {
            return referralService.DeleteReferalContact(referralContactId, modifiedOn);
        }
    }
}