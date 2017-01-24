using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Admin
{
    public class UserAdditionalDetailsModel
    {
        public UserAdditionalDetailsModel()
        {
            UserDetails = new List<UserAdditionalDetailsBaseModel>();
        }
        public List<UserAdditionalDetailsBaseModel> UserDetails { get; set; }
    }

    public class UserAdditionalDetailsBaseModel : BaseEntity
    {
        public long UserAdditionalDetailID { get; set; }
        public int UserID { get; set; }
        public string ContractingEntity { get; set; }
        public string IDNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
