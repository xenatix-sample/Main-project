using Axis.Model.Common;
using System;

namespace Axis.Model.Account
{
    public class AccessTokenModel : BaseEntity
    {
        public int AccessTokenID { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string Token { get; set; }
        public string ClientIP { get; set; }
        public string SessionID { get; set; }
        public Nullable<System.DateTime> GeneratedOn { get; set; }
        public Nullable<System.DateTime> ExpirationDate { get; set; }
    }
}
