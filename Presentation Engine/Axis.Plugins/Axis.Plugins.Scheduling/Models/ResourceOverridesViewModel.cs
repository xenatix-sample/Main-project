using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.Scheduling.Models
{
    public class ResourceOverridesViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the resource override identifier.
        /// </summary>
        /// <value>
        /// The resource override identifier.
        /// </value>
        public long ResourceOverrideID { get; set; }
        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public int ResourceID { get; set; }
        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public short ResourceTypeID { get; set; }
        /// <summary>
        /// Gets or sets the override date.
        /// </summary>
        /// <value>
        /// The override date.
        /// </value>
        public DateTime OverrideDate { get; set; }
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { get; set; }
        /// <summary>
        /// Gets or sets the facility identifier.
        /// </summary>
        /// <value>
        /// The facility identifier.
        /// </value>
        public int FacilityID { get; set; }
    }
}