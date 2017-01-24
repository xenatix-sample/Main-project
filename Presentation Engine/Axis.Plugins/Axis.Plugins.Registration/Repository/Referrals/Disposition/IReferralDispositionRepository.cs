using Axis.Model.Common;
using Axis.Plugins.Registration.Models.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository.Referrals.Disposition
{
    public interface IReferralDispositionRepository
    {

        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral disposition detail identifier.</param>
        /// <returns></returns>
        Response<ReferralDispositionViewModel> GetReferralDispositionDetail(long referralDispositionDetailID);

        /// <summary>
        /// Adds the referral disposition.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralDispositionViewModel> AddReferralDisposition(ReferralDispositionViewModel referral);

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral.</param>
        /// <returns></returns>
        Response<ReferralDispositionViewModel> UpdateReferralDisposition(ReferralDispositionViewModel referral);
    }
}
