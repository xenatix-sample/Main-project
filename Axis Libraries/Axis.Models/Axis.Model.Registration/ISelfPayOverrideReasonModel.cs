using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Registration
{
    public interface ISelfPayOverrideReasonModel
    {
        /// <summary>
        /// Gets or sets the isChild in conservatorship
        /// </summary>
        /// <value>
        /// The isChild in conservatorship.
        /// </value>
        bool? ISChildInConservatorship { get; set; }

        /// <summary>
        /// Gets or sets the isNot attested
        /// </summary>
        /// <value>
        /// The isNot attested
        /// </value>
        bool? IsNotAttested { get; set; }

        /// <summary>
        /// Gets or sets the isEnrolled in public benefits
        /// </summary>
        /// <value>
        /// The isEnrolled in public benefits
        /// </value>
        bool? IsEnrolledInPublicBenefits { get; set; }

        /// <summary>
        /// Gets or sets the isRequesting reconsideration
        /// </summary>
        /// <value>
        /// The isRequesting reconsideration
        /// </value>
        bool? IsRequestingReconsideration { get; set; }

        /// <summary>
        /// Gets or sets the IsNot giving consent
        /// </summary>
        /// <value>
        /// The IsNot giving consent
        /// </value>
        bool? IsNotGivingConsent { get; set; }

        /// <summary>
        /// Gets or sets the isOther child enrolled
        /// </summary>
        /// <value>
        /// The isOther child enrolled
        /// </value>
        bool? IsOtherChildEnrolled { get; set; }

        /// <summary>
        /// Gets or sets the isApplying for public benefits
        /// </summary>
        /// <value>
        /// The isApplying for public benefits
        /// </value>
        bool? IsApplyingForPublicBenefits { get; set; }

        /// <summary>
        /// Gets or sets the isReconsideration of adjustment
        /// </summary>
        /// <value>
        /// The isReconsideration of adjustment
        /// </value>
        bool? IsReconsiderationOfAdjustment { get; set; }
    }
}
