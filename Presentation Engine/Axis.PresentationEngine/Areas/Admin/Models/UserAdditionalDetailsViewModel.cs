using System;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Model
{
    public class UserAdditionalDetailsViewModel
    {
        public UserAdditionalDetailsViewModel()
        {
            UserDetails = new List<UserAdditionalDetailsViewBaseModel>();
        }
        public List<UserAdditionalDetailsViewBaseModel> UserDetails { get; set; }
    }

    public class UserAdditionalDetailsViewBaseModel : BaseViewModel
    {
        public long UserAdditionalDetailID { get; set; }
        public int UserID { get; set; }
        public string ContractingEntity { get; set; }
        public string IDNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
