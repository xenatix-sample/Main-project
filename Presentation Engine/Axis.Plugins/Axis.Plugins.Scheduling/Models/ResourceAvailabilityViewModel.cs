using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class ResourceAvailabilityViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the resource availability identifier.
        /// </summary>
        /// <value>
        /// The resource availability identifier.
        /// </value>
        public long ResourceAvailabilityID { get; set; }

        /// <summary>
        /// Gets or sets the resource identifier.
        /// </summary>
        /// <value>
        /// The resource identifier.
        /// </value>
        public int ResourceID { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public short ResourceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the facility identifier.
        /// </summary>
        /// <value>
        /// The facility identifier.
        /// </value>
        public int FacilityID { get; set; }

        /// <summary>
        /// Gets or sets the days.
        /// </summary>
        /// <value>
        /// The days.
        /// </value>
        public string Days { get; set; }

        /// <summary>
        /// Gets or sets the availability start time.
        /// </summary>
        /// <value>
        /// The availability start time.
        /// </value>
        public string AvailabilityStartTime { get; set; }

        /// <summary>
        /// Gets or sets the availability end time.
        /// </summary>
        /// <value>
        /// The availability end time.
        /// </value>
        public string AvailabilityEndTime { get; set; }

        /// <summary>
        /// Gets or sets the schedule type.
        /// </summary>
        public string ScheduleType { get; set; }
    }
}