using System.Collections.Generic;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Admin.Models.UserScheduling
{
    public class UserSchedulingViewModel : UserSchedulingBaseViewModel
    {
        public UserSchedulingViewModel()
        {
            UserFacilitySchedule = new List<UserFacilitySchedulingViewModel>();
            FacilitySchedule = new List<UserFacilitySchedulingViewModel>();
        }
       
        /// <summary>
        /// Gets or sets the Resource  identifier (Staff User ID).
        /// </summary>
        /// <value>
        /// The Resource identifier.
        /// </value>
        public int ResourceID { get; set; }

        public int RowCount { get; set; }

        public List<UserFacilitySchedulingViewModel> UserFacilitySchedule { get; set; }

        public List<UserFacilitySchedulingViewModel> FacilitySchedule { get; set; }

    }

    
}