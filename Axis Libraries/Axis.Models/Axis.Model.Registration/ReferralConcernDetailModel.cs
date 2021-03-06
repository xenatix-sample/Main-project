﻿using Axis.Model.Common;

namespace Axis.Model.Registration
{
    public class ReferralConcernDetailModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral concern detail identifier.
        /// </summary>
        /// <value>
        /// The referral concern detail identifier.
        /// </value>
        public long ReferralConcernDetailID { get; set; }
        /// <summary>
        /// Gets or sets the referral additional detail identifier.
        /// </summary>
        /// <value>
        /// The referral additional detail identifier.
        /// </value>
        public long? ReferralAdditionalDetailID { get; set; }
        /// <summary>
        /// Gets or sets the referral concern ID identifier.
        /// </summary>
        /// <value>
        /// The referral concernID identifier.
        /// </value>
        public int? ReferralConcernID { get; set; }
        /// <summary>
        /// Gets or sets the referral concern identifier.
        /// </summary>
        /// <value>
        /// The referral concern identifier.
        /// </value>
        public string ReferralConcern { get; set; }
        /// <summary>
        /// Gets or sets the referral priority identifier.
        /// </summary>
        /// <value>
        /// The referral priority identifier.
        /// </value>
        public int? ReferralPriorityID { get; set; }
        /// <summary>
        /// Gets or sets the diagnosis.
        /// </summary>
        /// <value>
        /// The diagnosis.
        /// </value>
        public string Diagnosis { get; set; }
    }
}
