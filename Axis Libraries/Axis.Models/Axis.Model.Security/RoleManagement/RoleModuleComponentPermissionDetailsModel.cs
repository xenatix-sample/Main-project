using Axis.Model.Common;

namespace Axis.Model.Security.RoleManagement
{
    public class RoleModuleComponentPermissionDetailsModel : BaseEntity
    {
        public long RoleModuleComponentPermissionID { get; set; }
        public long RoleModuleComponentID { get; set; }
        public int? PermissionLevelID { get; set; }
        public long PermissionID { get; set; }
    }
}
