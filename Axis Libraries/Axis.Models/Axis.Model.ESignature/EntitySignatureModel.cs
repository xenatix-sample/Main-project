using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.ESignature
{
    public class EntitySignatureModel : BaseEntity
    {
        public long EntitySignatureId { get; set; }
        public long EntityId { get; set; }
        public long? SignatureId { get; set; }
        public int EntityTypeId { get; set; }
        public int? SignatureTypeId { get; set; }
        public byte[] SignatureBlob { get; set; }
        public long? CredentialID { get; set; }
    }
}
