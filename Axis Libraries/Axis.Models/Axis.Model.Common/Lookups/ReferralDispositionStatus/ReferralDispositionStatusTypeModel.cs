using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common
{
    /// <summary>
    /// Class ReferralDispositionStatusTypeModel.
    /// </summary>
    public class ReferralDispositionStatusTypeModel
    {
        /// <summary>
        /// Gets or sets the referral disposition identifier.
        /// </summary>
        /// <value>The referral resource disposition identifier.</value>
        public int ReferralDispositionStatusID { get; set; }

        /// <summary>
        /// Gets or sets the type of the referral disposition.
        /// </summary>
        /// <value>The referral disposition</value>
        public string ReferralDispositionStatus { get; set; }
    }
}
