using Axis.Model.Common;

namespace Axis.Model.Registration
{
    public class ReferralContactModel : BaseEntity
    {
        /// <summary>
        /// Referreal Id
        /// </summary>
        public long ReferralID { get; set; }

        /// <summary>
        /// Referral Contact ID
        /// </summary>
        public long ReferralContactID { get; set; }

        /// <summary>
        /// Contact ID
        /// </summary>
        public long ContactID { get; set; }

    }
}