using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    public class AliasHistoryModel : BaseEntity
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
        /// Gets or sets the contact alias identifier.
        /// </summary>
        /// <value>
        /// The contact alias identifier.
        /// </value>
        public long? ContactAliasID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the alias.
        /// </summary>
        /// <value>
        /// The first name of the alias.
        /// </value>
        public string AliasFirstName { get; set; }

        /// <summary>
        /// Gets or sets the alias middle.
        /// </summary>
        /// <value>
        /// The alias middle.
        /// </value>
        public string AliasMiddle { get; set; }

        /// <summary>
        /// Gets or sets the last name of the alias.
        /// </summary>
        /// <value>
        /// The last name of the alias.
        /// </value>
        public string AliasLastName { get; set; }

        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix { get; set; }
    }
}
