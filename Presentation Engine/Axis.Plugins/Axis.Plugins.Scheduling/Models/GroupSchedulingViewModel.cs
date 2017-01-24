using System.Collections.Generic;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    public class GroupSchedulingViewModel : BaseViewModel
    {
        public GroupSchedulingViewModel()
        {
            GroupDetails = new List<GroupSchedulingDetailViewModel>();
        }

        public long? GroupHeaderID { get; set; }
        public long? GroupDetailID { get; set; }
        public string Comments { get; set; }
        public List<GroupSchedulingDetailViewModel> GroupDetails { get; set; }
    }
}
