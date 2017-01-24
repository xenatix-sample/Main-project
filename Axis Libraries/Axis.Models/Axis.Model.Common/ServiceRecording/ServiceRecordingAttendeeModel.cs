
namespace Axis.Model.Common.ServiceRecording
{
    public class ServiceRecordingAttendeeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the service recording attendee identifier.
        /// </summary>
        /// <value>
        /// The service recording attendee identifier.
        /// </value>
        public long ServiceRecordingAttendeeID { get; set; }

        /// <summary>
        /// Gets or sets the service recording identifier.
        /// </summary>
        /// <value>
        /// The service recording identifier.
        /// </value>
        public long ServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}
