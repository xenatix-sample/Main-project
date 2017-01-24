namespace Axis.Model.Clinical
{
    public class SocialRelationshipHistoryDetailsModel : ClinicalBaseModel
    {
        public long SocialRelationshipID { get; set; }
        /// <summary>
        /// SocialRelationshipDetailID: PK of detail table
        /// </summary>
        public long SocialRelationshipDetailID { get; set; }
        /// <summary>
        /// RelationshipTypeID
        /// </summary>
        public int RelationshipTypeID { get; set; }
        public long FamilyRelationshipID { get; set; }
        /// <summary>
        /// IsDeceased
        /// </summary>
        public bool IsDeceased { get; set; }
        /// <summary>
        /// IsInvolved
        /// </summary>
        public bool IsInvolved { get; set; }
        /// <summary>
        /// FirstName 
        /// </summary>
        public string FirstName { get; set; }
        /// <summary>
        /// Last Name
        /// </summary>
        public string LastName { get; set; }
    }
}
