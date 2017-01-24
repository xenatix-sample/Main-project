using Axis.Model.Common;
using Axis.Model.Registration.Referrals;
using Axis.Service.Registration.Referrals.Disposition;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.RuleEngine.Registration.Referrals.Disposition
{
    public class ReferralDispositionRuleEngine : IReferralDispositionRuleEngine
    {

        #region Class Variables
        /// <summary>
        /// The referral disposition service
        /// </summary>
        private readonly IReferralDispositionService referralDispositionService;

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferralDispositionRuleEngine"/> class.
        /// </summary>
        /// <param name="referralService">The referral disposition service.</param>
        public ReferralDispositionRuleEngine(IReferralDispositionService referralDispositionService)
        {
            this.referralDispositionService = referralDispositionService;
        }

        #endregion

        #region Public Methods
        /// <summary>
        /// Gets the referral disposition detail.
        /// </summary>
        /// <param name="referralOutcomeDetailID">The referral disposition detail identifier.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> GetReferralDispositionDetail(long referralHeaderID)
        {
            return referralDispositionService.GetReferralDispositionDetail(referralHeaderID);
        }
        /// <summary>
        /// Adds the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> AddReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            return referralDispositionService.AddReferralDisposition(referralDisposition);
        }

        /// <summary>
        /// Updates the referral disposition.
        /// </summary>
        /// <param name="referral">The referral disposition.</param>
        /// <returns></returns>
        public Response<ReferralDispositionModel> UpdateReferralDisposition(ReferralDispositionModel referralDisposition)
        {
            return referralDispositionService.UpdateReferralDisposition(referralDisposition);
        }
        #endregion
    }
}
