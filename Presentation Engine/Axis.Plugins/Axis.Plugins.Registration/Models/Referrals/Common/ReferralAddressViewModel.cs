using Axis.Plugins.Registration.Model;

namespace Axis.Plugins.Registration.Models.Referrals.Common
{
    public class ReferralAddressViewModel : AddressViewModel
    {
        /// <summary>
        /// Gets or sets the referral address identifier.
        /// </summary>
        /// <value>
        /// The referral address identifier.
        /// </value>
        public long ReferralAddressID { get; set; }

        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralID { get; set; }

        /// <summary>
        /// Gets or sets the mail permission identifier.
        /// </summary>
        /// <value>
        /// The mail permission identifier.
        /// </value>
        public int? MailPermissionID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }


    }
}
