namespace Axis.Model.Common
{
    /// <summary>
    /// Service Workflow Type Model
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ServiceWorkflowTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the module component identifier.
        /// </summary>
        /// <value>
        /// The module component identifier.
        /// </value>
        public long ModuleComponentID { get; set; }


        /// <summary>
        /// Gets or sets the feature.
        /// </summary>
        /// <value>
        /// The feature.
        /// </value>
        public string Feature { get; set; }
    }
}
