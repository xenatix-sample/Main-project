namespace Axis.Model.Clinical.SocialRelationship
{
    public class SocialRelationshipModel : ClinicalBaseModel
    {
        /// <summary>
        /// Gets or sets the SocialRelationshipID identifier.
        /// </summary>
        /// <value>
        /// The social relationship history identifier.
        /// </value>
        public long SocialRelationshipID { get; set; }
        /// <summary>
        /// ReviewedNoChanges
        /// </summary>
        public bool ReviewedNoChanges { get; set; }
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
