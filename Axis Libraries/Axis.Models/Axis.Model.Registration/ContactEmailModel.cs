using Axis.Model.Email;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Represents contact and email mapping
    /// </summary>
    public class ContactEmailModel : EmailModel
    {
        /// <summary>
        /// Gets or sets the contact email identifier.
        /// </summary>
        /// <value>
        /// The contact email identifier.
        /// </value>
        public long ContactEmailID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the email permission identifier.
        /// </summary>
        /// <value>
        /// The email permission identifier.
        /// </value>
        public int? EmailPermissionID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets the Effective Date.
        /// </summary>
        /// <value>
        /// Effective Date
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the Expiration Date.
        /// </summary>
        /// <value>
        /// Expiration Date
        /// </value>
        public DateTime? ExpirationDate { get; set; }
    }
}