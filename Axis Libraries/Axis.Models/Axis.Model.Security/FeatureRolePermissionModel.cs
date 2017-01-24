using Axis.Model.Common;
namespace Axis.Model.Security
{
    public class EntityRolePermissionModel : BaseEntity
    {
        public int EntityRolePermissionID { get; set; }
        public int RoleId { get; set; }
        public int EntityId { get; set; }
        public int PermissionId { get; set; }

        public virtual RoleModel Role { get; set; }
        public virtual EntityModel Entity { get; set; }
        public virtual PermissionModel Permission { get; set; }
    }
}
