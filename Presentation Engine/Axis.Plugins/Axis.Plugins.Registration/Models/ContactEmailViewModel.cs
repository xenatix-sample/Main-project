using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Model
{
    /// <summary>
    /// Represents Contact's Email
    /// </summary>
    public class ContactEmailViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the contact email identifier.
        /// </summary>
        /// <value>
        /// The contact email identifier.
        /// </value>
        public long ContactEmailID { get; set; }

        /// <summary>
        /// Contact ID
        /// </summary>
        public long ContactID { get; set; }


        /// <summary>
        /// Get Email id
        /// </summary>
        public long EmailID { get; set; }

        /// <summary>
        /// Get/Set email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        /// Get/Set isPrimary
        /// </summary>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Get/Set Eamil permissionID
        /// </summary>
        public int? EmailPermissionID { get; set; }

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
