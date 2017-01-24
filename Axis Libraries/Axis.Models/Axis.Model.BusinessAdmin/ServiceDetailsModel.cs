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
    public class ServiceDetailsModel : BaseEntity
    {
        public long ServicesModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the services identifier.
        /// </summary>
        /// <value>
        /// The services identifier.
        /// </value>
        public int ServicesID { get; set; }

        /// <summary>
        /// Gets or sets the module component identifier.
        /// </summary>
        /// <value>
        /// The module component identifier.
        /// </value>
        public long ModuleComponentID { get; set; }

        /// <summary>
        /// Gets or sets the delivery methods.
        /// </summary>
        /// <value>
        /// The delivery methods.
        /// </value>
        public List<DeliveryMethodModel> DeliveryMethods { get; set; }

        /// <summary>
        /// Gets or sets the service location.
        /// </summary>
        /// <value>
        /// The service location.
        /// </value>
        public List<ServiceLocationModel> ServiceLocation { get; set; }

        /// <summary>
        /// Gets or sets the recipients.
        /// </summary>
        /// <value>
        /// The recipients.
        /// </value>
        public List<RecipientCodeModel> Recipients { get; set; }

        /// <summary>
        /// Gets or sets the service status.
        /// </summary>
        /// <value>
        /// The service status.
        /// </value>
        public List<ServiceStatusModel> ServiceStatus { get; set; }

        /// <summary>
        /// Gets or sets the attendance status.
        /// </summary>
        /// <value>
        /// The attendance status.
        /// </value>
        public List<AttendanceStatusModel> AttendanceStatus { get; set; }

        /// <summary>
        /// Gets or sets the tracking field.
        /// </summary>
        /// <value>
        /// The tracking field.
        /// </value>
        public List<TrackingFieldModel> TrackingField { get; set; }

        /// <summary>
        /// Gets or sets the credentials.
        /// </summary>
        /// <value>
        /// The credentials.
        /// </value>
        public List<CredentialModel> Credentials { get; set; }


    }
}