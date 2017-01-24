namespace Axis.Model.Common
{
    public class ServiceStatusModel : BaseEntity
    {

        /// <summary>
        /// Gets or sets the service id.
        /// </summary>
        /// <value>
        /// The service id.
        /// </value>
        public int ServiceID { get; set; }

        /// <summary>
        /// Gets or sets the services status identifier.
        /// </summary>
        /// <value>
        /// The service status identifier.
        /// </value>
        public short ServiceStatusID { get; set; }

        /// <summary>
        /// Gets or sets the services status.
        /// </summary>
        /// <value>
        /// The services status.
        /// </value>
        public string ServiceStatus { get; set; }

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
