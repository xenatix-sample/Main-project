using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.BusinessAdmin.Models
{
    public class ClientMergeViewModel : BaseViewModel
    {
        public long ParentMRN { get; set; }

        public long ChildMRN { get; set; }

    }
}