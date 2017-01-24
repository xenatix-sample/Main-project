
using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Security.RoleManagement
{
    public class RoleModuleComponentModel : BaseEntity
    {
        public RoleModuleComponentModel()
        {
            ModuleComponents = new List<RoleModuleComponentDetailsModel>();
        }

        public string FeatureName { get; set; }

        public List<RoleModuleComponentDetailsModel> ModuleComponents { get; set; }

    }
}
