using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.Model.Registration.Referrals
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ReferralClientInformationModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the client demographics model.
        /// </summary>
        /// <value>
        /// The client demographics model.
        /// </value>
        public ContactDemographicsModel clientDemographicsModel { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        public List<ContactAddressModel> Addresses { get; set; }

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
        public ReferralClientConcernsModel Concern { get; set; }

        /// <summary>
        /// Gets or sets the referral client additional details.
        /// </summary>
        /// <value>
        /// The referral client additional details.
        /// </value>
        public ReferralClientAdditionalDetailsModel referralClientAdditionalDetails { get; set; }
    }
}
