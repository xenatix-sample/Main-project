using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class RoleModuleViewModel : BaseViewModel
    {
        public long RoleModuleID { get; set; }
        public long RoleId { get; set; }
        public string RoleName { get; set; }
        public long ModuleId { get; set; }
        public string ModuleName { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public bool Selected { get; set; }
    }
}