namespace Axis.Model.Common
{
    public class ReferralCategoryModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral category identifier.
        /// </summary>
        /// <value>
        /// The referral category identifier.
        /// </value>
        public int ReferralCategoryID { get; set; }
        /// <summary>
        /// Gets or sets the referral category.
        /// </summary>
        /// <value>
        /// The referral category.
        /// </value>
        public string ReferralCategory { get; set; }
    }
}
