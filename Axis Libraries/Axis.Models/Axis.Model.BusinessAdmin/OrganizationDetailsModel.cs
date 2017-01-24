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
    public class OrganizationDetailsModel : BaseEntity
    {
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
        /// Gets or sets the acronym.
        /// </summary>
        /// <value>
        /// The acronym.
        /// </value>
        public string Acronym { get; set; }

        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

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

        /// <summary>
        /// Gets or sets the is external.
        /// </summary>
        /// <value>
        /// The is external.
        /// </value>
        public Boolean? IsExternal { get; set; }
    }
}