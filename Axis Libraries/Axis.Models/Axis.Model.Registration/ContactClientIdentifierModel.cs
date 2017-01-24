using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ContactClientIdentifierModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the contact client identifier identifier.
        /// </summary>
        /// <value>
        /// The contact client identifier identifier.
        /// </value>
        public long ContactClientIdentifierID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the client identifier type identifier.
        /// </summary>
        /// <value>
        /// The client identifier type identifier.
        /// </value>
        public int? ClientIdentifierTypeID { get; set; }

        /// <summary>
        /// Gets or sets the alternate identifier.
        /// </summary>
        /// <value>
        /// The alternate identifier.
        /// </value>
        public string AlternateID { get; set; }

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
        /// Gets or sets the expiration reason.
        /// </summary>
        /// <value>
        /// The expiration reason.
        /// </value>
        public int? ExpirationReasonID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }
    }
}
