using Axis.Model.Common;

namespace Axis.Model.Security.RoleManagement
{
    public class RoleModulePermissionDetailsModel : BaseEntity
    {
        public long RoleModulePermissionID { get; set; }
        public long RoleModuleID { get; set; }
        public int? PermissionLevelID { get; set; }
        public long PermissionID { get; set; }
    }
}
