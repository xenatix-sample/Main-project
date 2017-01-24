namespace Axis.Plugins.Clinical.Models.Shared
{
    public class FamilyRelationshipViewModel : ClinicalBaseViewModel
    {
        public long FamilyRelationshipID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool IsDeceased { get; set; }
        public string Comments { get; set; }
    }
}
