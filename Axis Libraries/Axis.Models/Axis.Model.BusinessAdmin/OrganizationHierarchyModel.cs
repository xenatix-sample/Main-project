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
    public class OrganizationHierarchyModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the mapping identifier.
        /// </summary>
        /// <value>
        /// The mapping identifier.
        /// </value>
        public long? MappingID { get; set; }

        /// <summary>
        /// Gets or sets the company identifier.
        /// </summary>
        /// <value>
        /// The company identifier.
        /// </value>
        public long? CompanyID { get; set; }

        /// <summary>
        /// Gets or sets the name of the company.
        /// </summary>
        /// <value>
        /// The name of the company.
        /// </value>
        public string CompanyName { get; set; }

        /// <summary>
        /// Gets or sets the division identifier.
        /// </summary>
        /// <value>
        /// The division identifier.
        /// </value>
        public long? DivisionID { get; set; }

        /// <summary>
        /// Gets or sets the name of the division.
        /// </summary>
        /// <value>
        /// The name of the division.
        /// </value>
        public string DivisionName { get; set; }

        /// <summary>
        /// Gets or sets the program identifier.
        /// </summary>
        /// <value>
        /// The program identifier.
        /// </value>
        public long? ProgramID { get; set; }

        /// <summary>
        /// Gets or sets the name of the program.
        /// </summary>
        /// <value>
        /// The name of the program.
        /// </value>
        public string ProgramName { get; set; }

        /// <summary>
        /// Gets or sets the program unit identifier.
        /// </summary>
        /// <value>
        /// The program unit identifier.
        /// </value>
        public long? ProgramUnitID { get; set; }

        /// <summary>
        /// Gets or sets the name of the program unit.
        /// </summary>
        /// <value>
        /// The name of the program unit.
        /// </value>
        public string ProgramUnitName { get; set; }

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