using System.Collections.Generic;
using Axis.Plugins.Clinical.Models.Shared;

namespace Axis.Plugins.Clinical.Models.MedicalHistory
{
    public class MedicalHistoryConditionViewModel : ClinicalBaseViewModel
    {
        public MedicalHistoryConditionViewModel()
        {
            Details = new List<MedicalHistoryConditionDetailViewModel>();
        }

        public long? MedicalHistoryConditionID { get; set; }
        public long MedicalHistoryID { get; set; }
        public int MedicalConditionID { get; set; }
        public bool? HasCondition { get; set; }
        public int RowCount { get; set; }
        public List<MedicalHistoryConditionDetailViewModel> Details { get; set; }
    }
}
