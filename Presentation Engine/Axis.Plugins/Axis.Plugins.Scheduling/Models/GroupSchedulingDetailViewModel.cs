using System;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    public class GroupSchedulingDetailViewModel : BaseViewModel
    {
        public long GroupDetailID { get; set; }
        public string GroupName { get; set; }
        public Int16 GroupCapacity { get; set; }
        public int GroupTypeID { get; set; }
    }
}
