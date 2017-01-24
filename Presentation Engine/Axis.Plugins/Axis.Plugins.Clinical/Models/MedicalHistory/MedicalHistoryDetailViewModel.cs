using System.Collections.Generic;
using Axis.Model.Clinical.MedicalHistory;
using Axis.Plugins.Clinical.Models.Shared;

namespace Axis.Plugins.Clinical.Models.MedicalHistory
{
    public class MedicalHistoryDetailViewModel : ClinicalBaseViewModel
    {
        public MedicalHistoryDetailViewModel()
        {
            FamilyRelationships = new List<FamilyRelationshipModel>();
        }

        public long MedicalHistoryID { get; set; }
        public long MedicalHistoryConditionDetailID { get; set; }
        public long? MedicalHistoryConditionID { get; set; }
        public int MedicalConditionID { get; set; }
        public bool IsSystem { get; set; }
        public List<FamilyRelationshipModel> FamilyRelationships { get; set; }
    }
}
