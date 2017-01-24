using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Registration.Models
{
    public class ReferralAdditionalDetailViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the referral additional detail identifier.
        /// </summary>
        /// <value>
        /// The referral additional detail identifier.
        /// </value>
        public long ReferralAdditionalDetailID { get; set; }

        /// <summary>
        /// Gets or sets the referral header identifier.
        /// </summary>
        /// <value>
        /// The referral header identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the reasonfor care.
        /// </summary>
        /// <value>
        /// The reasonfor care.
        /// </value>
        public string ReasonforCare { get; set; }

        /// <summary>
        /// Gets or sets the is transferred.
        /// </summary>
        /// <value>
        /// The is transferred.
        /// </value>
        public bool? IsTransferred { get; set; }

        /// <summary>
        /// Gets or sets the is housing program.
        /// </summary>
        /// <value>
        /// The is housing program.
        /// </value>
        public bool? IsHousingProgram { get; set; }

        /// <summary>
        /// Gets or sets the housing description.
        /// </summary>
        /// <value>
        /// The housing description.
        /// </value>
        public string HousingDescription { get; set; }

        /// <summary>
        /// Gets or sets the is eligiblefor furlough.
        /// </summary>
        /// <value>
        /// The is eligiblefor furlough.
        /// </value>
        public bool? IsEligibleforFurlough { get; set; }

        /// <summary>
        /// Gets or sets the is referral discharge or transfer.
        /// </summary>
        /// <value>
        /// The is referral discharge or transfer.
        /// </value>
        public bool? IsReferralDischargeOrTransfer { get; set; }

        /// <summary>
        /// Gets or sets the is consent required.
        /// </summary>
        /// <value>
        /// The is consent required.
        /// </value>
        public bool? IsConsentRequired { get; set; }

        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { get; set; }

        /// <summary>
        /// Gets or sets the additional concerns.
        /// </summary>
        /// <value>
        /// The additional concerns.
        /// </value>
        public string AdditionalConcerns { get; set; }

        /// <summary>
        /// Gets or sets the header contactID.
        /// </summary>
        /// <value>
        /// The Header ContactID.
        /// </value>
        public long HeaderContactID { get; set; }
    }
}