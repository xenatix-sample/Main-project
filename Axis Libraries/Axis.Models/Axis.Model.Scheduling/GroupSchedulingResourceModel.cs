using System;
using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    public class GroupSchedulingResourceModel : BaseEntity
    {
        public GroupSchedulingResourceModel()
        {
            Appointments = new List<AppointmentModel>();
        }

        public long? GroupResourceID { get; set; }
        public long GroupHeaderID { get; set; }
        public int ResourceID { get; set; }
        public Int16 ResourceTypeID { get; set; }
        public bool IsPrimary { get; set; }
        public long? AppointmentResourceID { get; set; }
        public long? PrimaryAppointmentID { get; set; }
        public List<AppointmentModel> Appointments { get; set; } 
    }
}
