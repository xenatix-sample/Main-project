using Axis.Model.Phone;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Represents contact & phone mapping
    /// </summary>
    public class ContactPhoneModel : PhoneModel
    {
        /// <summary>
        /// Gets or sets the contact phone identifier.
        /// </summary>
        /// <value>
        /// The contact phone identifier.
        /// </value>
        public long ContactPhoneID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the phone permission identifier.
        /// </summary>
        /// <value>
        /// The phone permission identifier.
        /// </value>
        public int? PhonePermissionID { get; set; }

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