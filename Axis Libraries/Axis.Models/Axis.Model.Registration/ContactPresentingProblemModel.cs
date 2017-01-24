using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPresentingProblemModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the contact presenting problem identifier.
        /// </summary>
        /// <value>
        /// The contact presenting problem identifier.
        /// </value>
        public long ContactPresentingProblemID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public short? PresentingProblemTypeID { get; set; }

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
    }
}