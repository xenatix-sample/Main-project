using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Helpers
{
    public class PermissionAttribute : Attribute
    {
        public string Module { get; set; }
        public string Feature { get; set; }
        public string Action { get; set; }

        public PermissionAttribute()
        {
            
        }
    }
}
