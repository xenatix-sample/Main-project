using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Security
{
    public class UserPermissionModel : BaseEntity
    {
        public long? RoleModuleComponentID { get; set; }
        public long? RoleModuleID { get; set; }
        public long PermissionID { get; set; }
        public int? PermissionLevelID { get; set; }
        public string PermissionLevelName { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
    }
}
