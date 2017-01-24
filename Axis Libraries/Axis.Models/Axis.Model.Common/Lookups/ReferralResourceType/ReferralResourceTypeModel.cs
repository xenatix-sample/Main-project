namespace Axis.Model.Common
{
    public class ReferralResourceTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral resource type identifier.
        /// </summary>
        /// <value>
        /// The referral resource type identifier.
        /// </value>
        public int ReferralResourceTypeID { get; set; }
        /// <summary>
        /// Gets or sets the type of the referral resource.
        /// </summary>
        /// <value>
        /// The type of the referral resource.
        /// </value>
        public string ReferralResourceType { get; set; }
    }
}
