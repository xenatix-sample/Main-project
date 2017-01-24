using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class PermissionViewModel : BaseViewModel
    {
        public long PermissionID { get; set; }
        public int PermissionLevelID { get; set; }
        public string PermissionLevelName { get; set; }
        public string PermissionName { get; set; }
        public string Description { get; set; }
        public int Code { get; set; }
        public bool Selected { get; set; }
    }
}