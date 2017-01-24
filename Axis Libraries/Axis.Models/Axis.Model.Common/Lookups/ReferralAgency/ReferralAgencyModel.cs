namespace Axis.Model.Common
{
    public class ReferralAgencyModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral agency identifier.
        /// </summary>
        /// <value>
        /// The  referral agency identifier.
        /// </value>
        public int ReferralAgencyID { get; set; }

        /// <summary>
        /// Gets or sets the referral agency.
        /// </summary>
        /// <value>
        /// The referral agency.
        /// </value>
        public string ReferralAgency { get; set; }
    }
}
