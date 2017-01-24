using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Model.Clinical.MedicalHistory
{
    public class MedicalHistoryConditionModel : ClinicalBaseModel
    {
        public MedicalHistoryConditionModel()
        {
            Details = new List<MedicalHistoryConditionDetailModel>();
        }

        public long? MedicalHistoryConditionID { get; set; }
        public long MedicalHistoryID { get; set; }
        public int MedicalConditionID { get; set; }
        public bool? HasCondition { get; set; }
        public List<MedicalHistoryConditionDetailModel> Details { get; set; }
    }
}
