using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Admin
{
    public class CoSignaturesModel
    {
        public CoSignaturesModel()
        {
            CoSignatures = new List<CoSignaturesBaseModel>();
        }
        public List<CoSignaturesBaseModel> CoSignatures { get; set; }
    }

    public class CoSignaturesBaseModel : BaseEntity
    {
        public long CoSignatureID { get; set; }
        public int UserID { get; set; }
        public int CoSigneeID { get; set; }
        public int DocumentTypeGroupID { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
