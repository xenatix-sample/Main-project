using System;

namespace Axis.Plugins.Registration.Models.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    public class ReferralClientAdditionalDetailsViewModel
    {
        /// <summary>
        /// Gets or sets the referral additional detail identifier.
        /// </summary>
        /// <value>
        /// The referral additional detail identifier.
        /// </value>
        public long ReferralAdditionalDetailID { get; set; }

        /// <summary>
        /// Gets or sets the additional concerns.
        /// </summary>
        /// <value>
        /// The additional concerns.
        /// </value>
        public string AdditionalConcerns { get; set; }

        /// <summary>
        /// Gets or sets the referral header identifier.
        /// </summary>
        /// <value>
        /// The referral header identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [outof service area].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [outof service area]; otherwise, <c>false</c>.
        /// </value>
        public bool OutofServiceArea { get; set; }

        /// <summary>
        /// Gets or sets the dateof last physical.
        /// </summary>
        /// <value>
        /// The dateof last physical.
        /// </value>
        public DateTime? DateofLastPhysical { get; set; }

        /// <summary>
        /// Gets or sets the immunization status identifier.
        /// </summary>
        /// <value>
        /// The immunization status identifier.
        /// </value>
        public int? ImmunizationStatusID { get; set; }

        /// <summary>
        /// Gets or sets the is curently hospitalized.
        /// </summary>
        /// <value>
        /// The is curently hospitalized.
        /// </value>
        public bool? IsCurentlyHospitalized { get; set; }

        /// <summary>
        /// Gets or sets the expected discharge date.
        /// </summary>
        /// <value>
        /// The expected discharge date.
        /// </value>
        public DateTime? ExpectedDischargeDate { get; set; }

        /// <summary>
        /// Gets or sets the is CPS involved.
        /// </summary>
        /// <value>
        /// The is CPS involved.
        /// </value>
        public bool? SSI { get; set; }

    }
}
