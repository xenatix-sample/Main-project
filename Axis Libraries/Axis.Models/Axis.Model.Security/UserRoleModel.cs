using Axis.Model.Common;

namespace Axis.Model.Security
{
    public class UserRoleModel : BaseEntity
    {
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public long RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool HasRole { get; set; }
        public string RoleGUID { get; set; }
        public bool ADFlag { get; set; }
    }
}
