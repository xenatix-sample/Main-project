using System;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Admin.Model
{
    public class CoSignaturesViewModel
    {
        public CoSignaturesViewModel()
        {
            CoSignatures = new List<CoSignaturesViewBaseModel>();
        }
        public List<CoSignaturesViewBaseModel> CoSignatures { get; set; }
    }

    public class CoSignaturesViewBaseModel : BaseViewModel
    {
        public long CoSignatureID { get; set; }
        public int UserID { get; set; }
        public int CoSigneeID { get; set; }
        public int DocumentTypeGroupID { get; set; }
        public DateTime? EffectiveDate { get; set; }
        public DateTime? ExpirationDate { get; set; }
    }
}
