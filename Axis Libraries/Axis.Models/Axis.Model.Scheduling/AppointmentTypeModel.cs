using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentTypeModel : BaseEntity
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

        /// <summary>
        /// Get or set the Service ID
        /// </summary>
        public int ServiceID { get; set; }

        /// <summary>
        /// get or set the ServiceName
        /// </summary>
        public string ServiceName { get; set; }
    }
}