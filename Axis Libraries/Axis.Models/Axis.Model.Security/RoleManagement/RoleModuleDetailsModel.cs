using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Security.RoleManagement
{
    public class RoleModuleDetailsModel : BaseEntity
    {
        public long ModuleID { get; set; }
        public long RoleModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? PermissionLevelID { get; set; }
        public string RoleName { get; set; }
        public List<RoleModulePermissionDetailsModel> ModulePermissions { get; set; }
    }
}
