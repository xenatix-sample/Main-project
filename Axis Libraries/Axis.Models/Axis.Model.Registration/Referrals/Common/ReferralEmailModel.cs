using Axis.Model.Email;

namespace Axis.Model.Registration.Referrals.Common
{
    public class ReferralEmailModel : EmailModel
    {
        /// <summary>
        /// Gets or sets the referral email identifier.
        /// </summary>
        /// <value>
        /// The referral email identifier.
        /// </value>
        public long ReferralEmailID { get; set; }

        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralID { get; set; }              

        /// <summary>
        /// Gets or sets the email permission identifier.
        /// </summary>
        /// <value>
        /// The email permission identifier.
        /// </value>
        public int? EmailPermissionID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }
    }
}
