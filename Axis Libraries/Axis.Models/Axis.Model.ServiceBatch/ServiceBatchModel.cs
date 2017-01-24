using Axis.Model.Common;

namespace Axis.Model.ServiceBatch
{
    public class ServiceBatchModel : BaseEntity
    {
        public long BatchID { get; set; }
        public int BatchStatusID { get; set; }
        public int BatchTypeID { get; set; }
        public int ConfigID { get; set; }
        public long USN { get; set; }
        public string Filename { get; set; }
    }
}
