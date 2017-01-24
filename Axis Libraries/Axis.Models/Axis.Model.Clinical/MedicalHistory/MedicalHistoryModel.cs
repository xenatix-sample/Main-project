using System.Collections.Generic;

namespace Axis.Model.Clinical.MedicalHistory
{
    public class MedicalHistoryModel : ClinicalBaseModel
    {
        public MedicalHistoryModel()
        {
            Conditions = new List<MedicalHistoryConditionModel>();
        }

        public long MedicalHistoryID { get; set; }
        public List<MedicalHistoryConditionModel> Conditions { get; set; }
    }
}
