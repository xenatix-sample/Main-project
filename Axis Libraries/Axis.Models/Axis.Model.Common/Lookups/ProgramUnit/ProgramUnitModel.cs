
namespace Axis.Model.Common
{
    public class ProgramUnitModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the services identifier.
        /// </summary>
        /// <value>
        /// The services identifier.
        /// </value>
        public int ServicesID { get; set; }

        /// <summary>
        /// Gets or sets the name of the service.
        /// </summary>
        /// <value>
        /// The name of the service.
        /// </value>
        public string ServiceName { get; set; }

        /// <summary>
        /// Gets or sets the detail identifier.
        /// </summary>
        /// <value>
        /// The detail identifier.
        /// </value>
        public long OrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the data key.
        /// </summary>
        /// <value>
        /// The data key.
        /// </value>
        public string DataKey { get; set; }

        /// <summary>
        /// Gets or sets the detail identifier.
        /// </summary>
        /// <value>
        /// The detail identifier.
        /// </value>
        public long DetailID { get; set; }
    }
}
