using Axis.Model.Common;
using System;

namespace Axis.Model.Registration.Referral
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralOutcomeDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral outcome detail identifier.
        /// </summary>
        /// <value>
        /// The referral outcome detail identifier.
        /// </value>
        public long ReferralOutcomeDetailID { get; set; }

        /// <summary>
        /// Gets or sets the referral header identifier.
        /// </summary>
        /// <value>
        /// The referral header identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [followup expected].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [followup expected]; otherwise, <c>false</c>.
        /// </value>
        public bool FollowupExpected { get; set; }

        /// <summary>
        /// Gets or sets the followup provider identifier.
        /// </summary>
        /// <value>
        /// The followup provider identifier.
        /// </value>
        public int? FollowupProviderID { get; set; }

        /// <summary>
        /// Gets or sets the followup date.
        /// </summary>
        /// <value>
        /// The followup date.
        /// </value>
        public DateTime? FollowupDate { get; set; }

        /// <summary>
        /// Gets or sets the followup outcome.
        /// </summary>
        /// <value>
        /// The followup outcome.
        /// </value>
        public string FollowupOutcome { get; set; }

       
        /// <summary>
        /// Gets or sets a value indicating whether this instance is appointment notified.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is appointment notified; otherwise, <c>false</c>.
        /// </value>
        public bool IsAppointmentNotified { get; set; }

        /// <summary>
        /// Gets or sets the appointment notification method.
        /// </summary>
        /// <value>
        /// The appointment notification method.
        /// </value>
        public string AppointmentNotificationMethod { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { get; set; }
    }
}