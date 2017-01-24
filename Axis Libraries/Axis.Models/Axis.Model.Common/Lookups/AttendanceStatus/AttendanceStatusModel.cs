namespace Axis.Model.Common
{
    public class AttendanceStatusModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the attendance status identifier.
        /// </summary>
        /// <value>
        /// The attendance status identifier.
        /// </value>
        public short AttendanceStatusID { get; set; }

        /// <summary>
        /// Gets or sets the attendance status.
        /// </summary>
        /// <value>
        /// The attendance status.
        /// </value>
        public string AttendanceStatus { get; set; }

        /// <summary>
        /// Gets or sets the services id.
        /// </summary>
        /// <value>
        /// The services id.
        /// </value>
        public int ServicesID { get; set; }

        /// <summary>
        /// Gets or sets the module component id.
        /// </summary>
        /// <value>
        /// The module component id.
        /// </value>
        public long ModuleComponentID { get; set; }
    }
}
