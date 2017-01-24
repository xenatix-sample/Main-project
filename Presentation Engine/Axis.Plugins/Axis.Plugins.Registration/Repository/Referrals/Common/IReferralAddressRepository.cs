using System;
using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals.Common;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Repository.Referrals.Common
{
    public interface IReferralAddressRepository
    {
        /// <summary>
        /// Gets the addresses.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        Response<ReferralAddressViewModel> GetAddresses(long referralID, int contactTypeID);

        /// <summary>
        /// Adds and update addresses.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralAddressViewModel> AddUpdateAddresses(List<ReferralAddressViewModel> referral);

        /// <summary>
        /// Deletes the address.
        /// </summary>
        /// <param name="referralAddressID">The referral address identifier.</param>
        /// <returns></returns>
        Response<ReferralAddressViewModel> DeleteAddress(long referralAddressID, DateTime modifiedOn);
    }
}