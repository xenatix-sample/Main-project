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
    public class CompanyDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the company.
        /// </summary>
        /// <value>
        /// The company.
        /// </value>
        public OrganizationDetailsModel Company { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        public List<OrganizationAddressModel> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the company hierarchies.
        /// </summary>
        /// <value>
        /// The company hierarchies.
        /// </value>
        public List<OrganizationHierarchyModel> CompanyHierarchies { get; set; }

        /// <summary>
        /// Gets or sets the company identifiers.
        /// </summary>
        /// <value>
        /// The company identifiers.
        /// </value>
        public List<OrganizationIdentifiersModel> CompanyIdentifiers { get; set; }
    }
}