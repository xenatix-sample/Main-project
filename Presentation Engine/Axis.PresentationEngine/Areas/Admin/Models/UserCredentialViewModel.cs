using System;
using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Admin.Model
{
    public class UserCredentialViewModel : BaseViewModel
    {
        public long UserCredentialID { get; set; }
        public int UserID { get; set; }
        public long CredentialID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int? StateIssuedByID { get; set; }
        public bool? LicenseRequired { get; set; }
        public string LicenseNbr { get; set; }
        public DateTime? LicenseIssueDate { get; set; }
        public DateTime? LicenseExpirationDate { get; set; }
    }
}