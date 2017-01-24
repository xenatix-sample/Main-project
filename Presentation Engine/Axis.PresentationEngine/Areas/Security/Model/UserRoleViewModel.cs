using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class UserRoleViewModel : BaseViewModel
    {
        public int UserRoleID { get; set; }
        public int UserID { get; set; }
        public int RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string RoleGUID { get; set; }
        public bool ADFlag { get; set; }
    }
}