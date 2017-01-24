using Axis.Plugins.Clinical.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical
{
    public class SocialRelationshipHistoryViewModel : ClinicalBaseViewModel
    {
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
        /// <summary>
        /// RelationshipTypeID
        /// </summary>
        public int RelationshipTypeID { get; set; }
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
    }
}
