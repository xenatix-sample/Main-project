using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    /// Model for Room
    /// </summary>
    public class RoomViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the room identifier.
        /// </summary>
        /// <value>
        /// The room identifier.
        /// </value>
        public int RoomID { get; set; }

        /// <summary>
        /// Gets or sets the facility identifier.
        /// </summary>
        /// <value>
        /// The facility identifier.
        /// </value>
        public int FacilityID { get; set; }

        /// <summary>
        /// Gets or sets the name of the room.
        /// </summary>
        /// <value>
        /// The name of the room.
        /// </value>
        public string RoomName { get; set; }

        /// <summary>
        /// Gets or sets the room capacity.
        /// </summary>
        /// <value>
        /// The room capacity.
        /// </value>
        public int RoomCapacity { get; set; }

        /// <summary>
        /// Gets or sets the isschedulable flag.
        /// </summary>
        /// <value>
        /// Flag denoting this room as schedulable.
        /// </value>
        public bool IsSchedulable { get; set; }
    }
}