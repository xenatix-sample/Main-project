namespace Axis.Model.Common
{
    /// <summary>
    /// Service ConfigType Model
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ServiceConfigTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the service configuration service type identifier.
        /// </summary>
        /// <value>
        /// The service configuration service type identifier.
        /// </value>
        public int ServiceConfigServiceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the service configuration service.
        /// </summary>
        /// <value>
        /// The type of the service configuration service.
        /// </value>
        public string ServiceConfigServiceType { get; set; }
    }
}
