using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Security
{
    public class RoleSecurityModel : BaseEntity
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public long ModuleID { get; set; }
        public string ModuleName { get; set; }
        public long RoleModuleID { get; set; }
        public string DataKey { get; set; }
        public long ComponentID { get; set; }
        public string ComponentName { get; set; }
        public long RoleModuleComponentID { get; set; }
        public List<UserPermissionModel> ModulePermissions { get; set; }
        public List<UserPermissionModel> ComponentPermissions { get; set; }
    }
}