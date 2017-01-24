
namespace Axis.Model.Common.ServiceRecording
{
    public class ServiceRecordingAdditionalUserModel : BaseEntity
    {

        /// <summary>
        /// Gets or sets the service recording additional user identifier.
        /// </summary>
        /// <value>
        /// The service recording additional user identifier.
        /// </value>
        public long ServiceRecordingAdditionalUserID { get; set; }

        /// <summary>
        /// Gets or sets the service recording identifier.
        /// </summary>
        /// <value>
        /// The service recording identifier.
        /// </value>
        public long ServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserID { get; set; }
    }
}
