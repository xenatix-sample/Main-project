using System.Collections.Generic;
using Axis.Model.Common;

namespace Axis.Model.Clinical.MedicalHistory
{
    public class MedicalHistoryDetailModel : BaseEntity
    {
        public MedicalHistoryDetailModel()
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