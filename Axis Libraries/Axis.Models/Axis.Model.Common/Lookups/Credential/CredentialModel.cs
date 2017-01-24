using System;

namespace Axis.Model.Common
{
    public class CredentialModel : BaseEntity
    {
        public long ID { get; set; }
        public long Name { get; set; }
        public long CredentialID { get; set; }
        public string CredentialName { get; set; }
        public string CredentialCode { get; set; }
        public string CredentialAbbreviation { get; set; }
        public bool? LicenseRequired { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
        public bool? IsInternal { get; set; }
    }
}
