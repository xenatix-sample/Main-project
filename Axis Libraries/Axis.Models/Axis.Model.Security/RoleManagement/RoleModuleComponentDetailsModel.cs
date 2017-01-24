using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Security.RoleManagement
{
    public class RoleModuleComponentDetailsModel : BaseEntity
    {
        public long? RoleModuleComponentID { get; set; }
        public long? RoleModuleID { get; set; }
        public long ModuleComponentID { get; set; }
        public long ModuleID { get; set; }
        public string ModuleName { get; set; }
        public long? FeatureID { get; set; }
        public string FeatureName { get; set; }
        public long ComponentID { get; set; }
        public string ComponentName { get; set; }
        public int ComponentTypeID { get; set; }
        public string ComponentType { get; set; }
        public int? PermissionLevelID { get; set; }

        public List<RoleModuleComponentPermissionDetailsModel> ModuleComponentPermissions { get; set; }
    }
}
