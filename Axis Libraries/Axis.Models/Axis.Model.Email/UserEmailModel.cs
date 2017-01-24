using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Email
{
    public class UserEmailModel : EmailModel
    {
        public int UserID { get; set; }
        public long UserEmailID { get; set; }
        public int? EmailPermissionID { get; set; }
        public bool IsPrimary { get; set; }
    }
}
