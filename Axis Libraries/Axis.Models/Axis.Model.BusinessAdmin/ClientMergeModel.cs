using Axis.Model.Common;

namespace Axis.Model.BusinessAdmin
{
    public class ClientMergeModel : BaseEntity
    {
        public long ParentMRN { get; set; }

        public long ChildMRN { get; set; }
      
    }
}
