using Axis.Model.Common;
using System;

namespace Axis.Model.Cache
{
    public class ManifestModel : BaseEntity
    {
        public int ManifestID { get; set; }
        public string FilePath { get; set; }
        public int Version { get; set; }
        public int? SecurityRoleID { get; set; }
        public DateTime LastUpdated { get; set; }
    }
}
