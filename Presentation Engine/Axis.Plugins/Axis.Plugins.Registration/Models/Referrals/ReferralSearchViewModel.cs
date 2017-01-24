using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.Registration.Models
{
    /// <summary>
    /// ReferralSearch ViewModel
    /// </summary>
    /// <seealso cref="Axis.PresentationEngine.Helpers.Model.BaseViewModel" />
    public class ReferralSearchViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the referral header identifier.
        /// </summary>
        /// <value>
        /// The referral header identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }
        /// <summary>
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }
        /// <summary>
        /// Gets or sets the tkidsid.
        /// </summary>
        /// <value>
        /// The tkidsid.
        /// </value>
        public long? TKIDSID { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the contact.
        /// </summary>
        /// <value>
        /// The contact.
        /// </value>
        public string Contact { get; set; }
        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime DOB { get; set; }
        /// <summary>
        /// Gets or sets the type of the referral.
        /// </summary>
        /// <value>
        /// The type of the referral.
        /// </value>
        public string ReferralType { get; set; }
        /// <summary>
        /// Gets or sets the name of the requestor.
        /// </summary>
        /// <value>
        /// The name of the requestor.
        /// </value>
        public string RequestorName { get; set; }
        /// <summary>
        /// Gets or sets the program.
        /// </summary>
        /// <value>
        /// The program.
        /// </value>
        public string ProgramUnit { get; set; }
        /// <summary>
        /// Gets or sets the transfer referral date.
        /// </summary>
        /// <value>
        /// The transfer referral date.
        /// </value>
        public DateTime? TransferReferralDate { get; set; }
        /// <summary>
        /// Gets or sets the referral status.
        /// </summary>
        /// <value>
        /// The referral status.
        /// </value>
        public string ReferralStatus { get; set; }
        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }
        /// <summary>
        /// Gets or sets the header contact identifier.
        /// </summary>
        /// <value>
        /// The header contact identifier.
        /// </value>
        public long HeaderContactID { get; set; }
        /// <summary>
        /// Gets or sets the requestor contact.
        /// </summary>
        /// <value>
        /// The requestor contact.
        /// </value>
        public string RequestorContact { get; set; }
        /// <summary>
        /// Gets or sets the Forwarded To.
        /// </summary>
        /// <value>
        /// The Forwarded To.
        /// </value>
        public string ForwardedTo { get; set; }
        /// <summary>
        /// Gets or sets the Submitted By.
        /// </summary>
        /// <value>
        /// The Submitted By.
        /// </value>
        public string SubmittedBy { get; set; }
    }
}
