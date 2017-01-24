using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ESignature.Models
{
    public class EntitySignatureViewModel : BaseViewModel
    {
        public long EntitySignatureId { get; set; }
        public long EntityId { get; set; }
        public long? SignatureId { get; set; }
        public int EntityTypeId { get; set; }
        public int? SignatureTypeId { get; set; }
        public string SignatureBlob { get; set; }
        public long? CredentialID { get; set; }
    }
}
