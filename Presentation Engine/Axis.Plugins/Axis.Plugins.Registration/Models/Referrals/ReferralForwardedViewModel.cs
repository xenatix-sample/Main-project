using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.Registration.Models.Referrals
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralForwardedViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the referral Forwarded detail identifier.
        /// </summary>
        /// <value>
        /// The referral Forwarded detail identifier.
        /// </value>
        public long ReferralForwardedDetailID { get; set; }

        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }
        
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
        /// GEts or sets the Referral Sent Date
        /// </summary>
        public DateTime ReferralSentDate { get; set; }

        /// <summary>
        /// GEts or sets the FacilityID
        /// </summary>
        public long OrganizationID { get; set; }
    }
}