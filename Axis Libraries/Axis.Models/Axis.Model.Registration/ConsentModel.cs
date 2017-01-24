using System;
using Axis.Model.Common;

namespace Axis.Model.Registration
{
    public class ConsentModel : BaseEntity
    {
        public int? FormId { get; set; }
        public long? SignatureId { get; set; }
        public byte[] SignatureBlob { get; set; }
        public long? ContactId { get; set; }
        public string AuthorizedBy { get; set; }
        public string ContactName { get; set; }
        public DateTime? ContactDateofBirth { get; set; }
    }
}
