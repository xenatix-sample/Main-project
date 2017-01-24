using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Models.Referrals
{
    public class ReferralDispositionViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the referral disposition detail identifier.
        /// </summary>
        /// <value>
        /// The referral disposition detail identifier.
        /// </value>
        public long ReferralDispositionDetailID { get; set; }

        /// <summary>
        /// Gets or sets the referral header identifier.
        /// </summary>
        /// <value>
        /// The referral header identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the referral disposition identifier.
        /// </summary>
        /// <value>
        /// The referral disposition identifier.
        /// </value>
        public int? ReferralDispositionID { get; set; }

        /// <summary>
        /// Gets or sets a value reason for denial.
        /// </summary>
        /// <value>
        ///  The reason for denial.
        /// </value>
        public string ReasonforDenial { get; set; }

        /// <summary>
        /// Gets or sets the referral disposition outcome identifier.
        /// </summary>
        /// <value>
        /// The referral disposition outcome identifier.
        /// </value>
        public int? ReferralDispositionOutcomeID { get; set; }

        /// <summary>
        /// Gets or sets the additional notes.
        /// </summary>
        /// <value>
        /// The additional notes.
        /// </value>
        public string AdditionalNotes { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user indentifier.
        /// </value>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets the disposition date.
        /// </summary>
        /// <value>
        /// The disposition date.
        /// </value>
        public DateTime DispositionDate { get; set; }
    }
}
