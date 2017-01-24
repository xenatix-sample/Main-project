using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    public class IdHistoryModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the transaction log identifier.
        /// </summary>
        /// <value>
        /// The transaction log identifier.
        /// </value>
        public long? TransactionLogID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int? UserID { get; set; }

        /// <summary>
        /// Gets or sets the changed date.
        /// </summary>
        /// <value>
        /// The changed date.
        /// </value>
        public DateTime? ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        /// <value>
        /// The first name of the user.
        /// </value>
        public string UserFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        /// <value>
        /// The last name of the user.
        /// </value>
        public string UserLastName { get; set; }

        /// <summary>
        /// Gets or sets the contact client identifier identifier.
        /// </summary>
        /// <value>
        /// The contact client identifier identifier.
        /// </value>
        public long? ContactClientIdentifierID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }

        /// <summary>
        /// Gets or sets the type of the client identifier.
        /// </summary>
        /// <value>
        /// The type of the client identifier.
        /// </value>
        public string ClientIdentifierType { get; set; }

        /// <summary>
        /// Gets or sets the alternate identifier.
        /// </summary>
        /// <value>
        /// The alternate identifier.
        /// </value>
        public string AlternateID { get; set; }

        /// <summary>
        /// Gets or sets the expiration reason.
        /// </summary>
        /// <value>
        /// The expiration reason.
        /// </value>
        public string ExpirationReason { get; set; }

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
