using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentStatusViewModel : BaseViewModel
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