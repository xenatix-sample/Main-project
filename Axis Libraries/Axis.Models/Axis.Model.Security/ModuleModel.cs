using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Security
{
    public class ModuleModel : BaseEntity
    {
        public long ModuleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public List<PermissionModel> Permissions { get; set; }
    }
}
