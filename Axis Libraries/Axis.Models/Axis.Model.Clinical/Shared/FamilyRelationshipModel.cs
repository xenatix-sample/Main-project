using Axis.Model.Common;

namespace Axis.Model.Clinical.MedicalHistory
{
    public class FamilyRelationshipModel : BaseEntity
    {
        public long MedicalHistoryConditionDetailID { get; set; }
        public bool IsDeceased { get; set; }
        public long FamilyRelationshipID { get; set; }
        public int RelationshipTypeID { get; set; }
        public string Comments { get; set;}
        public string FirstName { get; set; }
        public string LastName { get; set; }
    }
}