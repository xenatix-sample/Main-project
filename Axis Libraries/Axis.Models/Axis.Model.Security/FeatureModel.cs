using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Security
{
    public class FeatureModel : BaseEntity
    {
        public long FeatureID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ModuleID { get; set; }
        public int? ParentFeatureID { get; set; }

        public List<PermissionModel> Permissions { get; set; }
    }
}
