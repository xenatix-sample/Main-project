using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;

namespace Axis.RuleEngine.Registration.Referrals.Common
{
    public interface IReferralAddressRuleEngine
    {
        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        Response<ReferralAddressModel> GetAddresses(long referralID, int contactTypeID);


        /// <summary>
        /// Adds and update addresses.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralAddressModel> AddUpdateAddresses(List<ReferralAddressModel> referral);

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralAddressID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralAddressModel> DeleteAddress(long referralAddressID, DateTime modifiedOn);
    }
}