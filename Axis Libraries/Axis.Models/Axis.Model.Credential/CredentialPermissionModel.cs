using Axis.Model.Common;

namespace Axis.Model.Credential
{
    public class CredentialPermissionModel : BaseEntity
    {
        public long CredentialPermission { get; set; }
        public long CredentialID { get; set; }
        public int PermissionID { get; set; }
    }
}
