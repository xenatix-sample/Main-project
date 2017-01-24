using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Admin.Models.UserScheduling
{
    public class UserFacilityTimeSchedulingViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the Resource Availability identifier.
        /// </summary>
        /// <value>
        /// The Resource Availability identifier.
        /// </value>
        public long ResourceAvailabilityID { get; set; }
        /// <summary>
        /// Gets or sets the AvailabilityStartTime.
        /// </summary>
        /// <value>
        /// The AvailabilityStartTime.
        /// </value>
        public string AvailabilityStartTime { get; set; }

        /// <summary>
        /// Gets or sets the AvailabilityEndTime.
        /// </summary>
        /// <value>
        /// The AvailabilityEndTime.
        /// </value>
        public string AvailabilityEndTime { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }

        public bool IsChecked { get; set; }

        public int RowNumber { get; set; }
    }
}