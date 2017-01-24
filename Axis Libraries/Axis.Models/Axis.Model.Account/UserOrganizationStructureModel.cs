using System;
using Axis.Model.Common;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Account
{
    public class UserOrganizationStructureModel : BaseEntity
    {
        public long MappingID { get; set; }
        public long DetailID { get; set; }
        public long? ParentID { get; set; }
        public string DataKey { get; set; }
    }
}
