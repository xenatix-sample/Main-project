namespace Axis.Plugins.Clinical.Models.Shared
{
    public class MedicalHistoryConditionDetailViewModel : ClinicalBaseViewModel
    {
        public long MedicalHistoryConditionDetailID { get; set; }
        public long? MedicalHistoryConditionID { get; set; }
        public long FamilyRelationshipID { get; set; }
        public bool IsSelf { get; set; }
        public string Comments { get; set; }
        public int? RelationshipTypeID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeceased { get; set; }
        public bool IsFirst { get; set; }
        public bool IsLast { get; set; }
        public int RowNumber { get; set; }
    }
}
