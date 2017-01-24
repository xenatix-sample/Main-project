using System;
using Axis.Model.Common;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;

namespace Axis.DataProvider.Registration.Referrals.Common
{
    public interface IReferralPhoneDataProvider
    {
        /// <summary>
        /// Gets the phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="contactTypeID">The contact type identifier.</param>
        /// <returns></returns>
        Response<ReferralPhoneModel> GetPhones(long referralID, int? contactTypeID);


        /// <summary>
        /// Add and update phones.
        /// </summary>
        /// <param name="referralID">The referral identifier.</param>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralPhoneModel> AddUpdatePhones(List<ReferralPhoneModel> referral);

        /// <summary>
        /// Deletes the referral phone.
        /// </summary>
        /// <param name="referralPhoneID"></param>
        /// <param name="modifiedOn"></param>
        /// <returns></returns>
        Response<ReferralPhoneModel> DeleteReferralPhone(long referralPhoneID, DateTime modifiedOn);
    }
}
