using Axis.Model.Common;
namespace Axis.Model.Security
{
    public class PermissionModel : BaseEntity
    {
        public long PermissionID { get; set; }
        public int PermissionLevelID { get; set; }
        public string PermissionLevelName { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int Code { get; set; }
        public bool Selected { get; set; }
    }
}
