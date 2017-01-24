using Axis.Model.Common;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
namespace Axis.Model.BusinessAdmin
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class DivisionDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the division.
        /// </summary>
        /// <value>
        /// The division.
        /// </value>
        public OrganizationDetailsModel Division { get; set; }

        /// <summary>
        /// Gets or sets the division hierarchies.
        /// </summary>
        /// <value>
        /// The division hierarchies.
        /// </value>
        public List<OrganizationHierarchyModel> DivisionHierarchies { get; set; }
    }
}