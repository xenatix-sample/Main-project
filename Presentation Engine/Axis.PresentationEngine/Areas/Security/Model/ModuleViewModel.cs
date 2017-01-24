using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class ModuleViewModel : BaseViewModel
    {
        public long ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
    }
}