using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentStatusDetailModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the appointment status detail identifier.
        /// </summary>
        public long AppointmentStatusDetailID { get; set; }

        /// <summary>
        /// Gets or sets the appointment status identifier.
        /// </summary>
        public int AppointmentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the appt resource id.
        /// </summary>
        public long AppointmentResourceID { get; set; }

        public bool IsCancelled { get; set; }
        public int CancelReasonID { get; set; }
        public string Comments { get; set; }
    }
}