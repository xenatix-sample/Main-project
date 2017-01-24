using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Security.Model
{
    public class CredentialSecurityViewModel : BaseViewModel
    {
        public long CredentialID { get; set; }
        public string CredentialName { get; set; }
        public int CredentialActionID { get; set; }
        public string CredentialAction { get; set; }
        public string CredentialActionForm { get; set; }
        public int? ServicesID { get; set; }
    }
}