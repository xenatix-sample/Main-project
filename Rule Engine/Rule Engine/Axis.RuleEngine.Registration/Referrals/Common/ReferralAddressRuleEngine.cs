using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using Axis.Service.Registration.Referrals.Common;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration.Referrals.Common
{
    public class ReferralAddressRuleEngine : IReferralAddressRuleEngine
    {
        /// <summary>
        /// The referral address
        /// </summary>
        private readonly IReferralAddressService referralAddress;


        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralAddressRuleEngine"/> class.
        /// </summary>
        /// <param name="referralAddress">The referral address.</param>
        public ReferralAddressRuleEngine(IReferralAddressService referralAddress)
        {
            this.referralAddress = referralAddress;
        }

        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        public Response<ReferralAddressModel> GetAddresses(long referralID, int contactTypeID)
        {
            return referralAddress.GetAddresses(referralID, contactTypeID);
        }

        /// <summary>
        /// Adds and update addresses.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        public Response<ReferralAddressModel> AddUpdateAddresses(List<ReferralAddressModel> referral)
        {
            return referralAddress.AddUpdateAddresses(referral);
        }

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        public Response<ReferralAddressModel> DeleteAddress(long referralID, DateTime modifiedOn)
        {
            return referralAddress.DeleteAddress(referralID, modifiedOn);
        }
    }
}