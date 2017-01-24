using Axis.Model.Phone;

namespace Axis.Model.Registration.Referrals.Common
{
    public class ReferralPhoneModel : PhoneModel
    {
        /// <summary>
        /// Gets or sets the referral phone identifier.
        /// </summary>
        /// <value>
        /// The referral phone identifier.
        /// </value>
        public long ReferralPhoneID { get; set; }

        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralID { get; set; }

        /// <summary>
        /// Gets or sets the phone permission identifier.
        /// </summary>
        /// <value>
        /// The phone permission identifier.
        /// </value>
        public int PhonePermissionID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }


    }
}
