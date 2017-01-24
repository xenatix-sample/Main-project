namespace Axis.Model.Common
{
    /// <summary>
    /// ReferralCategorySource Model
    /// </summary>
    public class ReferralCategorySourceModel : BaseEntity
    {
        /// <summary>
        /// ReferralCategorySource identifer
        /// </summary>
        public int ReferralCategorySourceID { get; set; }

        /// <summary>
        /// Category id
        /// </summary>
        public int ReferralCategoryID { get; set; }

        /// <summary>
        /// Referral source
        /// </summary>
        public string ReferralSource { get; set; }
    }
}
