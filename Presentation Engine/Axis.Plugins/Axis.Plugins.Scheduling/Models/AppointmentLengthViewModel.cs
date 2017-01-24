using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentLengthViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the appointment length identifier.
        /// </summary>
        /// <value>
        /// The appointment length identifier.
        /// </value>
        public short AppointmentLengthID { get; set; }

        /// <summary>
        /// Gets or sets the length of the appointment.
        /// </summary>
        /// <value>
        /// The length of the appointment.
        /// </value>
        public int AppointmentLength { get; set; }

        /// <summary>
        /// Gets or sets the appointment type identifier.
        /// </summary>
        /// <value>
        /// The appointment type identifier.
        /// </value>
        public int AppointmentTypeID { get; set; }
    }
}