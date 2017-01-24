using Axis.Model.Common;

namespace Axis.Model.Security
{
    public class CredentialSecurityModel : BaseEntity
    {
        public long CredentialID { get; set; }
        public string CredentialName { get; set; }
        public int CredentialActionID { get; set; }
        public string CredentialAction { get; set; }
        public string CredentialActionForm { get; set; }
        public int? ServicesID { get; set; }
    }
}
