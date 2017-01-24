using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Address
{
    public class UserAddressModel : AddressModel
    {
        public int UserID { get; set; }
        public long UserAddressID { get; set; }
        public int? MailPermissionID { get; set; }
        public bool IsPrimary { get; set; }
    }
}
