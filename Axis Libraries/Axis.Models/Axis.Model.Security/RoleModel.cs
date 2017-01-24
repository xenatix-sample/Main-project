using Axis.Model.Common;
using System;
namespace Axis.Model.Security
{
    public class RoleModel : BaseEntity
    {
        public long RoleID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long ID { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
