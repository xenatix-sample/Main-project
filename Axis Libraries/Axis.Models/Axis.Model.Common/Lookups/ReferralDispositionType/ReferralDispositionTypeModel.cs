using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common
{
    public class ReferralDispositionTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral disposition identifier.
        /// </summary>
        /// <value>
        /// The referral resource disposition identifier.
        /// </value>
        public int ReferralDispositionID { get; set; }

        /// <summary>
        /// Gets or sets the type of the referral disposition.
        /// </summary>
        /// <value>
        /// The referral disposition
        /// </value>
        public string ReferralDisposition { get; set; }
    }
}
