using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.DataProvider.Registration.Referrals.Disposition
{
    public interface IReferralDispositionDataProvider
    {

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral disposition detail identifier.</param>
        /// <returns></returns>
        Response<ReferralDispositionModel> GetReferralDispositionDetail(long referralHeaderID);

        /// <summary>
        /// Adds the referral disposition.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralDispositionModel> AddReferralDisposition(ReferralDispositionModel referral);

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralDispositionModel> UpdateReferralDisposition(ReferralDispositionModel referral);
    }
}
