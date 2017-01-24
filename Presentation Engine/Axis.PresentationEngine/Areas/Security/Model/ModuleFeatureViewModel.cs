using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class ModuleFeatureViewModel : BaseViewModel
    {
        public long ModuleFeatureID { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }

        public long FeatureID { get; set; }
        public string FeatureName { get; set; }
        public string Description { get; set; }
        public int? ParentFeatureID { get; set; }

        public List<PermissionViewModel> Permissions { get; set; }
    }
}