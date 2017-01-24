using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common
{
    public class ReferralDispositionOutcomeTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral disposition outcome identifier.
        /// </summary>
        /// <value>
        /// The referral resource disposition outcome identifier.
        /// </value>
        public int ReferralDispositionOutcomeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the referral disposition outcome.
        /// </summary>
        /// <value>
        /// The referral disposition outcome
        /// </value>
        public string ReferralDispositionOutcome { get; set; }
    }
}
