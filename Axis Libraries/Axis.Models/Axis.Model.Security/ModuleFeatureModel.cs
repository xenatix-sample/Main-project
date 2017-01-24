using Axis.Model.Common;
using System.Collections.Generic;
namespace Axis.Model.Security
{
    public class ModuleFeatureModel : BaseEntity
    {
        public long ModuleFeatureID { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }

        public long FeatureID { get; set; }
        public string FeatureName { get; set; }
        public string Description { get; set; }
        public int? ParentFeatureID { get; set; }

        public List<PermissionModel> Permissions { get; set; }
    }
}
