using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentContactViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the appointment contact identifier.
        /// </summary>
        /// <value>
        /// The appointment contact identifier.
        /// </value>
        public long AppointmentContactID { get; set; }

        /// <summary>
        /// Gets or sets the appointment identifier.
        /// </summary>
        /// <value>
        /// The appointment identifier.
        /// </value>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }
    }
}