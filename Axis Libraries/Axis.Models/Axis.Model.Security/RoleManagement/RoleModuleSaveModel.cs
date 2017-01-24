
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Security.RoleManagement
{
    public class RoleModuleSaveModel : BaseEntity
    {
        public RoleModuleDetailsModel roleModule { get; set; }

        public List<RoleModuleComponentModel> roleModuleComponents { get; set; }
    }
}
