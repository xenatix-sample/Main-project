using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Models.Referrals.Common
{
    public class ReferralEmailViewModel : EmailViewModel
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
    }
}
