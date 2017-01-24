using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Model.Security
{
    public class RolePermissionsModel : BaseEntity
    {
        public RoleModel Role { get; set; }
        public List<ModuleModel> Modules { get; set; }
        public List<FeatureModel> Features { get; set; }
    }
}
