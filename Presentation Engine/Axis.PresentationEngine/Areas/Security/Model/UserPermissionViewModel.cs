using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class UserPermissionViewModel : BaseViewModel
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