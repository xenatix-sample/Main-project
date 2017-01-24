using System.Collections.Generic;

namespace Axis.Plugins.Clinical.Models.MedicalHistory
{
    public class MedicalHistoryViewModel : ClinicalBaseViewModel
    {
        public long MedicalHistoryID { get; set; }
        public List<MedicalHistoryConditionViewModel> Conditions { get; set; }
    }
}
