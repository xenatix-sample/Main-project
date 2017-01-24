using System;

namespace Axis.Plugins.Registration.Models
{
    /// <summary>
    /// Viewmodel for referral
    /// </summary>
    public class ReferralViewModel : ReferralContactViewModel
    {
        /// <summary>
        /// Referral name
        /// </summary>
        public string ReferralName { get; set; }

        /// <summary>
        /// Referral Organization
        /// </summary>
        public string ReferralOrganization { get; set; }

        /// <summary>
        /// Referral Category ID
        /// </summary>
        public int? ReferralCategoryID { get; set; }

        /// <summary>
        /// Referral Source ID
        /// </summary>
        public int? ReferralSourceID { get; set; }

        /// <summary>
        /// Referral Origin ID
        /// </summary>
        public int? ReferralOriginID { get; set; }

        /// <summary>
        /// Referral Program ID
        /// </summary>
        public int? ReferralProgramID { get; set; }

        /// <summary>
        /// Referral ClosureReason ID
        /// </summary>
        public int? ReferralClosureReasonID { get; set; }

        /// <summary>
        /// Referral Concern ID
        /// </summary>
        public string ReferralConcern { get; set; }

        /// <summary>
        /// Contact Name
        /// </summary>
        public string ContactName { get; set; }

        /// <summary>
        /// Organization
        /// </summary>
        public string Organization { get; set; }

        /// <summary>
        /// Referred Date
        /// </summary>
        public DateTime ReferredDate { get; set; }

        /// <summary>
        /// Program Name
        /// </summary>
        public string ProgramName { get; set; }
    }
}
