namespace Axis.Model.Common
{

    public class ReferralTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral type identifier.
        /// </summary>
        /// <value>
        /// The referral type identifier.
        /// </value>
        public int ReferralTypeID { get; set; }
        /// <summary>
        /// Gets or sets the type of the referral.
        /// </summary>
        /// <value>
        /// The type of the referral.
        /// </value>
        public string ReferralType { get; set; }
    }
}
