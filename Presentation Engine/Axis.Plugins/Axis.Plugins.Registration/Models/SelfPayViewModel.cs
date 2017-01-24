using Axis.Model.Common;
using System;

namespace Axis.Plugins.Registration.Models
{
    public class SelfPayViewModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the self pay identifier.
        /// </summary>
        /// <value>
        /// The self pay identifier.
        /// </value>
        public long SelfPayID { get; set; }

        /// <summary>
        /// Gets or sets the organisation detail identifier.
        /// </summary>
        /// <value>
        /// The organisation detail identifier.
        /// </value>
        public long OrganizationDetailID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the self pay amount.
        /// </summary>
        /// <value>
        /// The self pay amount.
        /// </value>
        public decimal? SelfPayAmount { get; set; }

        /// <summary>
        /// Gets or sets the is percent.
        /// </summary>
        /// <value>
        /// The is percent.
        /// </value>
        public bool? IsPercent { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the isChild in conservatorship
        /// </summary>
        /// <value>
        /// The isChild in conservatorship.
        /// </value>
        public bool? ISChildInConservatorship { get; set; }

        /// <summary>
        /// Gets or sets the isNot attested
        /// </summary>
        /// <value>
        /// The isNot attested
        /// </value>
        public bool? IsNotAttested { get; set; }

        /// <summary>
        /// Gets or sets the isEnrolled in public benefits
        /// </summary>
        /// <value>
        /// The isEnrolled in public benefits
        /// </value>
        public bool? IsEnrolledInPublicBenefits { get; set; }

        /// <summary>
        /// Gets or sets the isRequesting reconsideration
        /// </summary>
        /// <value>
        /// The isRequesting reconsideration
        /// </value>
        public bool? IsRequestingReconsideration { get; set; }

        /// <summary>
        /// Gets or sets the IsNot giving consent
        /// </summary>
        /// <value>
        /// The IsNot giving consent
        /// </value>
        public bool? IsNotGivingConsent { get; set; }

        /// <summary>
        /// Gets or sets the isOther child enrolled
        /// </summary>
        /// <value>
        /// The isOther child enrolled
        /// </value>
        public bool? IsOtherChildEnrolled { get; set; }

        /// <summary>
        /// Gets or sets the isApplying for public benefits
        /// </summary>
        /// <value>
        /// The isApplying for public benefits
        /// </value>
        public bool? IsApplyingForPublicBenefits { get; set; }

        /// <summary>
        /// Gets or sets the isReconsideration of adjustment
        /// </summary>
        /// <value>
        /// The isReconsideration of adjustment
        /// </value>
        public bool? IsReconsiderationOfAdjustment { get; set; }

        /// <summary>
        /// Gets or sets the is view selfPay
        /// </summary>
        /// <value>
        /// The is view selfPay
        /// </value>
        public bool? IsViewSelfPay { get; set; }
    }
}