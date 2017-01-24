using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Models.UserScheduling
{
    public class UserFacilitySchedulingViewModel : UserSchedulingBaseViewModel
    {
        public UserFacilitySchedulingViewModel()
        {

        }

        public int DayOfWeekID { get; set; }
        /// <summary>
        /// Gets or sets the Day.
        /// </summary>
        /// <value>
        /// The Day of the week.
        /// </value>
        public string Days { get; set; }

        public short? ScheduleTypeID { get; set; }

        public bool IsFirst { get; set; }

        public bool IsLast { get; set; }

        public int RowCount { get; set; }

        public List<UserFacilityTimeSchedulingViewModel> UserFacilityTimeSchedule { get; set; }


    }
}