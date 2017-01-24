using Axis.Model.Common;
using Axis.Model.Security;
using System;
using System.Collections.Generic;

namespace Axis.Model.Account
{
    public class UserModel : BaseEntity
    {        
        public int UserID { get; set; }
        public string UserGUID { get; set; }
        public bool ADFlag { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public Nullable<DateTime> EffectiveFromDate { get; set; }
        public Nullable<DateTime> EffectiveToDate { get; set; }
        public int? LoginAttempts { get; set; }
        public int? LoginCount { get; set; }
        public Nullable<DateTime> LastLogin { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int? GenderID { get; set; }
        public List<RoleModel> Roles { get; set; }
        public List<UserCredentialModel> Credentials { get; set; }
        public string IPAddress { get; set; }
        public string SessionID { get; set; }        
        public string PrimaryEmail { get; set; }
        public long? EmailID { get; set; }
        public string ADUserPasswordResetMessage { get; set; }
        public byte[] ThumbnailBLOB { get; set; }
        public bool? IsInternal { get; set; }

        public int DaysToExpire { get; set; }
    }
}
