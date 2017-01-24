using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration.Referrals.Disposition
{
    public interface IReferralDispositionRuleEngine
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
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        Response<ReferralDispositionModel> AddReferralDisposition(ReferralDispositionModel referralDisposition);
        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        Response<ReferralDispositionModel> UpdateReferralDisposition(ReferralDispositionModel referralDisposition);
    }
}
