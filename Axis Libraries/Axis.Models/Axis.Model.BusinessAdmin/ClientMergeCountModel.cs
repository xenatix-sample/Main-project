using Axis.Model.Common;

namespace Axis.Model.BusinessAdmin
{
    public class ClientMergeCountModel : BaseEntity
    {
        public int PotentialContactMatchCount { get; set; }

        public int MergedContactCount { get; set; }
    }
}
