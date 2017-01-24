using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Models.Referrals.Common
{
    public class ReferralPhoneViewModel : PhoneViewModel
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
    }
}
