using System.Collections.Generic;

namespace Axis.Model.Clinical
{
    public class SocialRelationshipHistoryModel : ClinicalBaseModel
    {
        #region Constrcutors

        public SocialRelationshipHistoryModel()
        {
            Details = new List<SocialRelationshipHistoryDetailsModel>();
        }

        #endregion

        #region Properties

        /// <summary>
        /// SocialRelationshipDetailID
        /// </summary>
        public long SocialRelationshipDetailID { get; set; }
        /// <summary>
        /// SocialRelationShipID
        /// </summary>
        public long SocialRelationshipID { get; set; }
        /// <summary>
        /// FamilyRelationshipID
        /// </summary>
        public long FamilyRelationshipID { get; set; }
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
        /// <summary>
        /// IsDetailsDirty
        /// </summary>
        public bool IsDetailsDirty { get; set; }
        /// <summary>
        /// IsSocialRelationshipDirty
        /// </summary>
        public bool IsSocialRelationshipDirty { get; set; }
        /// <summary>
        /// ReviewedNoChanges
        /// </summary>
        public bool ReviewedNoChanges { get; set; }
        /// <summary>
        /// IsFamilyRelationshipDirty
        /// </summary>
        public bool IsFamilyRelationshipDirty { get; set; }
        /// <summary>
        /// List of all detail records
        /// </summary>
        public List<SocialRelationshipHistoryDetailsModel> Details { get; set; }

        #endregion
    }
}
