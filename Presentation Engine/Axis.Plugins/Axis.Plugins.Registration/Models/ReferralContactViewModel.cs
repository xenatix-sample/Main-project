using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Registration.Models
{
    public class ReferralContactViewModel : BaseViewModel
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