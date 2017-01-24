using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    public class AppointmentTypeViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the appointment type identifier.
        /// </summary>
        /// <value>
        /// The appointment type identifier.
        /// </value>
        public int AppointmentTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the appointment.
        /// </summary>
        /// <value>
        /// The type of the appointment.
        /// </value>
        public string AppointmentType { get; set; }

        /// <summary>
        /// Gets or sets the program identifier.
        /// </summary>
        /// <value>
        /// The program identifier.
        /// </value>
        public long ProgramID { get; set; }
    }
}