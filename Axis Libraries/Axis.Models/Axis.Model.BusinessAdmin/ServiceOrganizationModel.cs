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
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ServiceOrganizationModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the services organization details identifier.
        /// </summary>
        /// <value>
        /// The services organization details identifier.
        /// </value>
        public int? ServicesOrganizationDetailsID { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public long? OrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the services identifier.
        /// </summary>
        /// <value>
        /// The services identifier.
        /// </value>
        public int? ServiceID { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }


        /// <summary>
        /// Gets or sets the detail identifier.
        /// </summary>
        /// <value>
        /// The detail identifier.
        /// </value>
        public long? DetailID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data key.
        /// </summary>
        /// <value>
        /// The data key.
        /// </value>
        public string DataKey { get; set; }
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