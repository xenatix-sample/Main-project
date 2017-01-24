using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Account
{
    public class ServerResourceModel : BaseEntity
    {
        public int ServerResourceID { get; set; }
        public string ServerResourceType { get; set; }
        public string ServerIP { get; set; }

    }
}
