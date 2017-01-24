using Axis.Model.Common;
namespace Axis.Model.Security
{
    public class ModulePermissionModel : BaseEntity
    {
        public int ModulePermissionID { get; set; }
        public int ModuleId { get; set; }
        public string ModuleName { get; set; }
        public int PermissionId { get; set; }
        public string PermissionName { get; set; }

        public virtual ModuleModel Module { get; set; }
        public virtual PermissionModel Permission { get; set; }
    }
}
