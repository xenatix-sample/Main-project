
using System.Collections.Generic;
namespace Axis.Model.Admin.UserScheduling
{
    public class UserFacilitySchedulingModel : UserSchedulingBaseModel
    {
        public UserFacilitySchedulingModel()
        {
            UserFacilityTimeSchedule = new List<UserFacilityTimeSchedulingModel>();
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

        public List<UserFacilityTimeSchedulingModel> UserFacilityTimeSchedule { get; set; }

    }
}
