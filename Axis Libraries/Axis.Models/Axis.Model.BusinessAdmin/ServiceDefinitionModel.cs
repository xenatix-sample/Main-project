using Axis.Model.Common;
using System;
using System.Collections.Generic;

/// <summary>
///
/// </summary>
namespace Axis.Model.BusinessAdmin
{
    /// <summary>
    /// Service Definition Model
    /// </summary>
    /// <seealso cref="Axis.Model.BusinessAdmin.ServicesModel" />
    public class ServiceDefinitionModel : ServicesModel
    {

        /// <summary>
        /// Gets or sets the expiration reason.
        /// </summary>
        /// <value>
        /// The expiration reason.
        /// </value>
        public string ExpirationReason { get; set; }

        /// <summary>
        /// Gets or sets the service definition.
        /// </summary>
        /// <value>
        /// The service definition.
        /// </value>
        public string ServiceDefinition { get; set; }

        /// <summary>
        /// Gets or sets the program unit hierarchies.
        /// </summary>
        /// <value>
        /// The program unit hierarchies.
        /// </value>
        public List<ServiceOrganizationModel> ProgramUnitHierarchies { get; set; }

        /// <summary>
        /// Gets or sets the notes.
        /// </summary>
        /// <value>
        /// The notes.
        /// </value>
        public string Notes { get; set; }


    }
}