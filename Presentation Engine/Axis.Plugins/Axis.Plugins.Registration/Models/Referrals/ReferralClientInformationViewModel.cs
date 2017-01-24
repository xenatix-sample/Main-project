using Axis.Model.Registration;
using Axis.Model.Registration.Referral;
using Axis.Plugins.Registration.Models.Referrals.Common;
using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Models.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Model.BaseViewModel" />
    public class ReferralClientInformationViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralID { get; set; }

        /// <summary>
        /// Gets or sets the client demographics model.
        /// </summary>
        /// <value>
        /// The client demographics model.
        /// </value>
        public ReferralDemographicsModel clientDemographicsModel { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        public List<ReferralAddressViewModel> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the phones.
        /// </summary>
        /// <value>
        /// The phones.
        /// </value>
        public List<ContactPhoneModel> Phones { get; set; }

        /// <summary>
        /// Gets or sets the concerns.
        /// </summary>
        /// <value>
        /// The concerns.
        /// </value>
        public List<ReferralClientConcernsViewModel> Concerns { get; set; }

        /// <summary>
        /// Gets or sets the referral client additional details.
        /// </summary>
        /// <value>
        /// The referral client additional details.
        /// </value>
        public ReferralClientAdditionalDetailsViewModel referralClientAdditionalDetails { get; set; }
    }
}
