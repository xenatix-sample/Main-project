using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Admin
{
    public class UserIdentifierDetailsModel
    {
        public UserIdentifierDetailsModel()
        {
            UserDetails = new List<UserIdentifierDetailsBaseModel>();
        }
        public List<UserIdentifierDetailsBaseModel> UserDetails { get; set; }
    }

    public class UserIdentifierDetailsBaseModel : BaseEntity
    {
        public long UserIdentifierDetailsID { get; set; }
        public int UserID { get; set; }
        public int UserIdentifierTypeID { get; set; }
        public string IDNumber { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
