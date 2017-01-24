using Axis.Model.Common;

namespace Axis.Model.BusinessAdmin
{
    /// <summary>
    /// Service Details Data Model
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ServiceDetailsDataModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the services module component identifier.
        /// </summary>
        /// <value>
        /// The services module component identifier.
        /// </value>
        public long ServicesModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the module component identifier.
        /// </summary>
        /// <value>
        /// The module component identifier.
        /// </value>
        public long ModuleComponentID { get; set; }


        /// <summary>
        /// Gets or sets the services identifier.
        /// </summary>
        /// <value>
        /// The services identifier.
        /// </value>
        public int ServicesID { get; set; }

        /// <summary>
        /// Gets or sets the service workflow.
        /// </summary>
        /// <value>
        /// The service workflow.
        /// </value>
        public string ServiceWorkflow { get; set; }

        /// <summary>
        /// Gets or sets the delivery method.
        /// </summary>
        /// <value>
        /// The delivery method.
        /// </value>
        public string DeliveryMethod { get; set; }

        /// <summary>
        /// Gets or sets the place of service.
        /// </summary>
        /// <value>
        /// The place of service.
        /// </value>
        public string PlaceOfService { get; set; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>
        /// The recipient.
        /// </value>
        public string Recipient { get; set; }

        /// <summary>
        /// Gets or sets the service status.
        /// </summary>
        /// <value>
        /// The service status.
        /// </value>
        public string ServiceStatus { get; set; }

        /// <summary>
        /// Gets or sets the attendance status.
        /// </summary>
        /// <value>
        /// The attendance status.
        /// </value>
        public string AttendanceStatus { get; set; }

        /// <summary>
        /// Gets or sets the tracking field.
        /// </summary>
        /// <value>
        /// The tracking field.
        /// </value>
        public string TrackingField { get; set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>
        /// The credentials.
        /// </value>
        public string Credentials { get; set; }


    }
}