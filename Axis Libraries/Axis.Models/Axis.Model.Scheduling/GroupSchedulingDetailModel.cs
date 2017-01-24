using System;
using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    public class GroupSchedulingDetailModel : BaseEntity
    {
        public long GroupDetailID { get; set; }
        public string GroupName { get; set; }
        public Int16 GroupCapacity { get; set; }
        public int GroupTypeID { get; set; }
    }
}
