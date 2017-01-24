
namespace Axis.Model.Common
{
    public class TrackingFieldModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the tracking field identifier.
        /// </summary>
        /// <value>
        /// The tracking field identifier.
        /// </value>
        public int TrackingFieldID { get; set; }

        /// <summary>
        /// Gets or sets the tracking field.
        /// </summary>
        /// <value>
        /// The tracking field.
        /// </value>
        public string TrackingField { get; set; }

        /// <summary>
        /// Gets or sets the services id.
        /// </summary>
        /// <value>
        /// The services id.
        /// </value>
        public int ServicesID { get; set; }

        /// <summary>
        /// Gets or sets the module component id.
        /// </summary>
        /// <value>
        /// The module component id.
        /// </value>
        public long ModuleComponentID { get; set; }

    }
}
