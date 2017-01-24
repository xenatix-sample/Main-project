
using Axis.Model.Common;

namespace Axis.Model.Registration.Referrals
{
    public class ReferralClientConcernsModel : BaseEntity
    {
        public long ReferralConcernDetailID { get; set; }

        public int? ReferralConcernID { get; set; }

        public long? ReferralAdditionalDetailID { get; set; }

        public string Diagnosis { get; set; }

        public int? ReferralPriorityID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is deleted.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is deleted; otherwise, <c>false</c>.
        /// </value>
        public bool IsDeleted { get; set; }
    }
}
