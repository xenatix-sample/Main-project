using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentContactModel : BaseEntity
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

        /// <summary>
        /// Gets or sets the contact's first name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the contact's last name
        /// </summary>
        public string LastName { get; set; }
    }
}