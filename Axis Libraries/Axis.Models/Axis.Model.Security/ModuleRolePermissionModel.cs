using Axis.Model.Common;
namespace Axis.Model.Security
{
    public class ModuleRolePermissionModel : BaseEntity
    {
        public int ModuleRolePermissionID { get; set; }
        public int RoleId { get; set; }
        public int ModuleId { get; set; }
        public int PermissionId { get; set; }

        public virtual RoleModel Role { get; set; }
        public virtual ModuleModel Module { get; set; }
        public virtual PermissionModel Permission { get; set; }
    }
}
