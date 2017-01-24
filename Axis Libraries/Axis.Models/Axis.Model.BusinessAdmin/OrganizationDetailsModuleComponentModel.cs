using Axis.Model.Common;
using System;

/// <summary>
///
/// </summary>
namespace Axis.Model.BusinessAdmin
{
    public class OrganizationDetailsModuleComponentModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the organization details module component identifier.
        /// </summary>
        /// <value>
        /// The organization details module component identifier.
        /// </value>
        public long? OrganizationDetailsModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the detail identifier.
        /// </summary>
        /// <value>
        /// The detail identifier.
        /// </value>
        public long? DetailID { get; set; }

        /// <summary>
        /// Gets or sets the module component identifier.
        /// </summary>
        /// <value>
        /// The module component identifier.
        /// </value>
        public long? ModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the feature.
        /// </summary>
        /// <value>
        /// The feature.
        /// </value>
        public string Feature { get; set; }

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