namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ReferralStatusModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral status identifier.
        /// </summary>
        /// <value>
        /// The referral status identifier.
        /// </value>
        public int ReferralStatusID { get; set; }

        /// <summary>
        /// Gets or sets the referral status.
        /// </summary>
        /// <value>
        /// The referral status.
        /// </value>
        public string ReferralStatus { get; set; }
    }
}