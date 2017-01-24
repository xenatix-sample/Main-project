using Axis.Model.Common;
using System;

namespace Axis.Model.Registration.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ReferralDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral header identifier.
        /// </summary>
        /// <value>
        /// The referral header identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the referral date.
        /// </summary>
        /// <value>
        /// The referral date.
        /// </value>
        public DateTime ReferralDate { get; set; }

        /// <summary>
        /// Gets or sets the referral source identifier.
        /// </summary>
        /// <value>
        /// The referral source identifier.
        /// </value>
        public int ReferralSourceID { get; set; }

        /// <summary>
        /// Gets or sets the referral concern.
        /// </summary>
        /// <value>
        /// The referral concern.
        /// </value>
        public string ReferralConcern { get; set; }

        /// <summary>
        /// Gets or sets the is referrer converted to contact.
        /// </summary>
        /// <value>
        /// The is referrer converted to contact.
        /// </value>
        public bool? IsReferrerConvertedToCollateral { get; set; }
    }
}