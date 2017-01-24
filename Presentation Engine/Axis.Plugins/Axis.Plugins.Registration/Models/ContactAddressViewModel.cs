using Axis.Model.Address;
using Axis.Plugins.Registration.Models;
using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Model
{
    /// <summary>
    /// Represents Contact Address  view model
    /// </summary>
    public class ContactAddressViewModel : AddressViewModel
    {
        /// <summary>
        /// Gets or sets the contact address identifier.
        /// </summary>
        /// <value>
        /// The contact address identifier.
        /// </value>
        public long ContactAddressID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the mail permission identifier.
        /// </summary>
        /// <value>
        /// The mail permission identifier.
        /// </value>
        public int? MailPermissionID { get; set; }

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