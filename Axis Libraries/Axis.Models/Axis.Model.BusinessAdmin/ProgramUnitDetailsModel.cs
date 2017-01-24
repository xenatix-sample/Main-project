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
    public class ProgramUnitDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the program unit.
        /// </summary>
        /// <value>
        /// The program unit.
        /// </value>
        public OrganizationDetailsModel ProgramUnit { get; set; }

        /// <summary>
        /// Gets or sets the reporting unit.
        /// </summary>
        /// <value>
        /// The reporting unit.
        /// </value>
        public OrganizationIdentifiersModel ReportingUnit { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        public List<OrganizationAddressModel> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the program unit hierarchies.
        /// </summary>
        /// <value>
        /// The program unit hierarchies.
        /// </value>
        public List<OrganizationHierarchyModel> ProgramUnitHierarchies { get; set; }

        /// <summary>
        /// Gets or sets the program unit services.
        /// </summary>
        /// <value>
        /// The program unit services.
        /// </value>
        public List<ServiceOrganizationModel> ProgramUnitServices { get; set; }

        /// <summary>
        /// Gets or sets the program unit service workflows.
        /// </summary>
        /// <value>
        /// The program unit service workflows.
        /// </value>
        public List<OrganizationDetailsModuleComponentModel> ProgramUnitServiceWorkflows { get; set; }
    }
}