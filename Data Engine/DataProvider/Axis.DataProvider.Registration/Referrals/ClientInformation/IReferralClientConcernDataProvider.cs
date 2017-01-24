using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Model.Registration.Referrals.Common;
using System.Collections.Generic;

namespace Axis.DataProvider.Registration.Referrals.ClientInformation
{
    public interface IReferralClientConcernDataProvider
    {
        /// <summary>
        /// Gets the Referral Client Concerns
        /// </summary>
        /// <param name="ReferralAdditionalDetailID">The referral AdditionalDetail identifier.</param>
        /// <returns></returns>
        Response<ReferralClientConcernsModel> GetClientConcern(long ReferralAdditionalDetailID);

       /// <summary>
        /// Add/Update the Referral Client Concerns
        /// </summary>
        /// <param name="ReferralAdditionalDetailID">The referral Client Concerns.</param>
        /// <returns></returns>
        Response<ReferralClientConcernsModel> AddUpdateClientConcern(ReferralClientConcernsModel clientConcern);
    }
}
