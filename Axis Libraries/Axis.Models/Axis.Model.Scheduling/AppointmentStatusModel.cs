using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentStatusModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the appointment status identifier.
        /// </summary>
        public int AppointmentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the status of the appointment.
        /// </summary>
        public string AppointmentStatus { get; set; }
    }
}