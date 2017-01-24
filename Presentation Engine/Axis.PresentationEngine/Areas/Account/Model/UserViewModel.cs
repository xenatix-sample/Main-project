using Axis.Model.Security;
using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Axis.PresentationEngine.Areas.Admin.Model;
using Axis.Model.Account;

namespace Axis.PresentationEngine.Areas.Account.Model
{
    public class UserViewModel : BaseViewModel
    {
        public int UserID { get; set; }
        public string UserGUID { get; set; }
        public bool ADFlag { get; set; }
        [Required(ErrorMessage = "Please Enter User Name")]
        public string UserName { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string MiddleName { get; set; }
        public int? GenderID { get; set; }
        [Required(ErrorMessage = "Please Enter Password")]
        public string Password { get; set; }
        public Nullable<DateTime> EffectiveFromDate { get; set; }
        public Nullable<DateTime> EffectiveToDate { get; set; }
        public List<RoleModel> Roles { get; set; }
        public List<UserCredentialViewModel> Credentials { get; set; }
        public int? LoginAttempts { get; set; }
        public int? LoginCount { get; set; }
        public Nullable<DateTime> LastLogin { get; set; }
        public string IPAddress { get; set; }
        public string SessionID { get; set; }
        public string PrimaryEmail { get; set; }
        public long? EmailID { get; set; }
        public byte[] ThumbnailBLOB { get; set; }
        public bool? IsInternal { get; set; }
    }
}