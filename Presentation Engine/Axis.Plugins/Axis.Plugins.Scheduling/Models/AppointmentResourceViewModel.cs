using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentResourceViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the appointment resource identifier.
        /// </summary>
        /// <value>
        /// The appointment resource identifier.
        /// </value>
        public long AppointmentResourceID { get; set; }

        /// <summary>
        /// Gets or sets the appointment identifier.
        /// </summary>
        /// <value>
        /// The appointment identifier.
        /// </value>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public short ResourceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public int ResourceID { get; set; }

        /// <summary>
        /// Gets or sets the parent identifier.
        /// </summary>
        /// <value>
        /// The parent identifier.
        /// </value>
        public long? ParentID { get; set; }

        /// <summary>
        /// Gets or sets the isnoshow flag.
        /// </summary>
        public bool IsNoShow { get; set; }

        /// <summary>
        /// Gets or sets the group header ID
        /// </summary>
        public long? GroupHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the Appointment Status identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public int? AppointmentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the Appointment Status detail identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public long? AppointmentStatusDetailID { get; set; }
    }
}