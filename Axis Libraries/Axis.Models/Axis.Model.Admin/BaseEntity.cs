using System;

namespace Axis.Model.Admin
{
    public abstract class BaseEntity
    {
        public Nullable<DateTime> ModifiedOn { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public Nullable<bool> IsActive { get; set; }
        public Nullable<bool> ForceRollback { get; set; }
    }
}