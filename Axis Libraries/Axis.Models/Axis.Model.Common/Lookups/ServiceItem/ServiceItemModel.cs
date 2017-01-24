namespace Axis.Model.Common
{
    public class ServiceItemModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the services identifier.
        /// </summary>
        /// <value>
        /// The services identifier.
        /// </value>
        public int ServiceID { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the program identifier.
        /// </summary>
        /// <value>
        /// The program identifier.
        /// </value>
        public long ProgramID { get; set; }

    }
}