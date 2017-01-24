using Axis.Model.Common;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
namespace Axis.Model.BusinessAdmin
{
    public class ProgramDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        public OrganizationDetailsModel Program { get; set; }

        /// <summary>
        /// Gets or sets the program hierarchies.
        /// </summary>
        /// <value>
        /// The program hierarchies.
        /// </value>
        public List<OrganizationHierarchyModel> ProgramHierarchies { get; set; }

        /// <summary>
        /// Gets or sets the division hierarchies.
        /// </summary>
        /// <value>
        /// The division hierarchies.
        /// </value>
        public List<OrganizationHierarchyModel> DivisionHierarchies { get; set; }
    }
}