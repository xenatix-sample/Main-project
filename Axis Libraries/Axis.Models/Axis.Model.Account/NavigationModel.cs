using Axis.Model.Common;
using Axis.Model.Security;
using System.Collections.Generic;

namespace Axis.Model.Account
{
    public class NavigationModel : BaseEntity
    {
        public int UserID { get; set; }
        public string UserFullName { get; set; }
        public string UserRolePrimary { get; set; }
        public bool IsProfileComplete { get; set; }
        public bool IsSecurityQuestionComplete { get; set; }
        public string PasswordHash { get; set; }
        public string UserName { get; set; }
        public string ContactNumber { get; set; }
        public string Extension { get; set; }
        public long? UserPhoneID { get; set; }
        public long? ContactNumberID { get; set; }
        public long? CredentialID { get; set; }
        public byte[] ThumbnailBLOB { get; set; }
        public string DigitalPassword { get; set; }
        public List<UserOrganizationStructureModel> UserOrganizationStructures { get; set; }
        public List<UserCredentialModel> UserCredentials { get; set; }
        public string PrintSignature { get; set; }
    }
}
