namespace Axis.Plugins.Clinical.Models.SocialRelationship
{
    public class SocialRelationshipViewModel : ClinicalBaseViewModel
    {
        /// <summary>
        /// Gets or sets the  Social Relationship  identifier.
        /// </summary>
        /// <value>
        /// The Social Relationship identifier.
        /// </value>
        public long SocialRelationshipID { get; set; }
        /// <summary>
        /// ChildhoodHistory
        /// </summary>
        public string ChildhoodHistory { get; set; }
        /// <summary>
        /// RelationShipHistory
        /// </summary>
        public string RelationShipHistory { get; set; }
        /// <summary>
        /// FamilyHistory
        /// </summary>
        public string FamilyHistory { get; set; }
    }
}
