using Axis.Model.Common;
namespace Axis.Model.Security
{
    public class CredentialModel : BaseEntity
    {
        public long CredentialID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
    }
}
