using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class RoleSecurityViewModel : BaseViewModel
    {
        public long RoleID { get; set; }
        public string RoleName { get; set; }
        public long ModuleID { get; set; }
        public string ModuleName { get; set; }
        public long RoleModuleID { get; set; }
        public string DataKey { get; set; }
        public long ComponentID { get; set; }
        public string ComponentName { get; set; }
        public List<UserPermissionViewModel> ModulePermissions { get; set; }
        public List<UserPermissionViewModel> ComponentPermissions { get; set; }
    }
}