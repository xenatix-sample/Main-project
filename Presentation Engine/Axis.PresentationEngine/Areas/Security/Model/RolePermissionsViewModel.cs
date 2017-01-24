using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class RolePermissionsViewModel
    {
        public RoleViewModel Role { get; set; }
        public List<ModuleViewModel> Modules { get; set; }
        public List<FeatureViewModel> Features { get; set; }
    }
}