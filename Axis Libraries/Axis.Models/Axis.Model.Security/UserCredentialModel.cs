using System;
using Axis.Model.Common;

namespace Axis.Model.Security
{
    public class UserCredentialModel : BaseEntity
    {
        public long UserCredentialID { get; set; }
        public int UserID { get; set; }
        public long CredentialID { get; set; }
        public string CredentialName { get; set; }
        public string Description { get; set; }
        public bool? LicenseRequired { get; set; }
        public string LicenseNbr { get; set; }
        public int? StateIssuedByID { get; set; }
        public DateTime? LicenseIssueDate { get; set; }
        public DateTime? LicenseExpirationDate { get; set; }
        public int ServicesID { get; set; }
    }
}
