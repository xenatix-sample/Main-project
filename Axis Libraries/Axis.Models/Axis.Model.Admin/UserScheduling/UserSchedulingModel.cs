using System.Collections.Generic;

namespace Axis.Model.Admin.UserScheduling
{
    public class UserSchedulingModel : UserSchedulingBaseModel
    {
        public UserSchedulingModel()
        {
            UserFacilitySchedule = new List<UserFacilitySchedulingModel>();
            FacilitySchedule = new List<UserFacilitySchedulingModel>();
        }

        /// <summary>
        /// Gets or sets the Resource  identifier (Staff User ID).
        /// </summary>
        /// <value>
        /// The Resource identifier.
        /// </value>
        public int ResourceID { get; set; }

        

        public List<UserFacilitySchedulingModel> UserFacilitySchedule { get; set; }

        public List<UserFacilitySchedulingModel> FacilitySchedule { get; set; }

    }

   
}