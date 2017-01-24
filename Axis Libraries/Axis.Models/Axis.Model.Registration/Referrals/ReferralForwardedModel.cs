using Axis.Model.Common;
using System;

namespace Axis.Model.Registration.Referrals
{
   public class ReferralForwardedModel : BaseEntity
    {
        /// <summary>
        ///Gets or sets the Referral Forwarded Detail ID
        /// </summary> 
        public long ReferralForwardedDetailID { get; set; }
        
        /// <summary>
        /// Gets or sets the Referral ID
        /// </summary>
        public long ReferralHeaderID { get; set; }
        
        /// <summary>
        /// Gets or sets the Facility ID
        /// </summary>
        public long OrganizationID { get; set; }
        
        /// <summary>
        /// Gets or sets the Sending referral to details
        /// </summary>
        public int? SendingReferralToID { get; set; }

        /// <summary>
        /// Gets or sets the Comments
        /// </summary>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the UserID
        /// </summary>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the Referral Sent Date
        /// </summary>
        public DateTime ReferralSentDate { get; set; }
    }
}
