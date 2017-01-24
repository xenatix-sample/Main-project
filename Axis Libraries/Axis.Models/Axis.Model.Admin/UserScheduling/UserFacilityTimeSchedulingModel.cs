using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Admin.UserScheduling
{
    public class UserFacilityTimeSchedulingModel : BaseEntity
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
    }
}
