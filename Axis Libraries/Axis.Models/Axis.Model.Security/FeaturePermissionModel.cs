using Axis.Model.Common;
namespace Axis.Model.Security
{
    public class EntityPermissionModel : BaseEntity
    {
        public int EntityPermissionID { get; set; }
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }

        public virtual EntityModel Entity { get; set; }
        public virtual PermissionModel Permission { get; set; }
    }
}
