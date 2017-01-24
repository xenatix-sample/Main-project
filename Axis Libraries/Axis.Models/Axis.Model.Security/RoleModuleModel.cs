using Axis.Model.Common;
using System.Collections.Generic;
namespace Axis.Model.Security
{
    public class RoleModuleModel : BaseEntity
    {
        public long? RoleModuleID { get; set; }
        public long? RoleId { get; set; }
        public string RoleName { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public bool Selected { get; set; }
    }
}
