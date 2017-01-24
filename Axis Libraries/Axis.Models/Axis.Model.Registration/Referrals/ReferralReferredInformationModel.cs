using Axis.Model.Common;
using System;

namespace Axis.Model.Registration.Referral
{
    /// <summary>
    /// Referral referred to information model
    /// </summary>
    public class ReferralReferredInformationModel : BaseEntity
    {
        /// <summary>
        ///Gets or sets the Referral detail ID
        /// </summary> 
        public long ReferredToDetailID { get; set; }

        /// <summary>
        /// Gets or sets the ReferralHeader ID
        /// </summary>
        public long ReferralHeaderID { get; set; }  

        /// <summary>
        /// Gets or sets the action taken
        /// </summary>
        public string ActionTaken { get; set; }

        /// <summary>
        /// Gets or sets the Comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// GEts or sets the referred datetime
        /// </summary>
        public DateTime? ReferredDateTime { get; set; }       

        /// <summary>
        /// Gets or sets the Programe ID
        /// </summary>
        public long? OrganizationID { get; set; }


        /// <summary>
        /// Gets or sets the Contact No
        /// </summary>
        public int ContactNo { get; set; }
    }
}
