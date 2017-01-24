using Axis.Model.Common;
using System;

/// <summary>
///
/// </summary>
namespace Axis.Model.BusinessAdmin
{
    /// <summary>
    ///
    /// </summary>
    public class OrganizationIdentifiersModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the organization identifier identifier.
        /// </summary>
        /// <value>
        /// The organization identifier identifier.
        /// </value>
        public int? OrganizationIdentifierID { get; set; }

        /// <summary>
        /// Gets or sets the detail identifier.
        /// </summary>
        /// <value>
        /// The detail identifier.
        /// </value>
        public long? DetailID { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier type identifier.
        /// </summary>
        /// <value>
        /// The organization identifier type identifier.
        /// </value>
        public int OrganizationIdentifierTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the organization identifier.
        /// </summary>
        /// <value>
        /// The type of the organization identifier.
        /// </value>
        public string OrganizationIdentifierType { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public string OrganizationIdentifier { get; set; }

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