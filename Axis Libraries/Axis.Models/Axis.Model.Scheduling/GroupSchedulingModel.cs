using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    public class GroupSchedulingModel : BaseEntity
    {
        public GroupSchedulingModel()
        {
            GroupDetails = new List<GroupSchedulingDetailModel>();
        }

        public long? GroupHeaderID { get; set; }
        public long? GroupDetailID { get; set; }
        public string Comments { get; set; }
        public List<GroupSchedulingDetailModel> GroupDetails { get; set; }
    }
}
