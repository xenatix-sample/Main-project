using System;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Model
{
    public class UserIdentifierViewModel
    {
        public UserIdentifierViewModel()
        {
            UserDetails = new List<UserIdentifierViewBaseModel>();
        }
        public List<UserIdentifierViewBaseModel> UserDetails { get; set; }
    }

    public class UserIdentifierViewBaseModel : BaseViewModel
    {
        public long UserIdentifierDetailsID { get; set; }
        public int UserID { get; set; }
        public int UserIdentifierTypeID { get; set; }
        public string IDNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
