using Axis.Model.Common;
using System;

/// <summary>
///
/// </summary>
namespace Axis.Model.BusinessAdmin
{
    /// <summary>
    /// Services Model
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ServicesModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the services identifier.
        /// </summary>
        /// <value>
        /// The services identifier.
        /// </value>
        public int? ServicesID { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the service code.
        /// </summary>
        /// <value>
        /// The service code.
        /// </value>
        public string ServiceCode { get; set; }

        /// <summary>
        /// Gets or sets the service configuration service type identifier.
        /// </summary>
        /// <value>
        /// The service configuration service type identifier.
        /// </value>
        public int? ServiceConfigServiceTypeID { get; set; }

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
        /// Gets or sets a value indicating whether [encounter reportable].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [encounter reportable]; otherwise, <c>false</c>.
        /// </value>
        public bool? EncounterReportable { get; set; }
    }
}