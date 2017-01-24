using Axis.Model.Registration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.ECI.Models.ECIDemographics
{
    /// <summary>
    /// Represents contact demography view model
    /// </summary>
    public class ECIContactDemographicsViewModel : ECIContactBaseViewModel
    {
        /// <summary>
        /// Gets or sets the client type identifier.
        /// </summary>
        /// <value>
        /// The client type identifier.
        /// </value>
        public int? ClientTypeID { get; set; }

        /// <summary>
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public int? TitleID { get; set; }

        /// <summary>
        /// Gets or sets the sequestered by identifier.
        /// </summary>
        /// <value>
        /// The sequestered by identifier.
        /// </value>
        public int? SequesteredByID { get; set; }

        /// <summary>
        /// Gets or sets the dob status identifier.
        /// </summary>
        /// <value>
        /// The dob status identifier.
        /// </value>
        public int? DOBStatusID { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        /// <value>
        /// The SSN.
        /// </value>
        public string SSN { get; set; }

        /// <summary>
        /// Gets or sets the SSN status identifier.
        /// </summary>
        /// <value>
        /// The SSN status identifier.
        /// </value>
        public int? SSNStatusID { get; set; }

        /// <summary>
        /// Gets or sets the alternate identifier.
        /// </summary>
        /// <value>
        /// The alternate identifier.
        /// </value>
        public string AlternateID { get; set; }

        /// <summary>
        /// Gets or sets the client identifier type identifier.
        /// </summary>
        /// <value>
        /// The client identifier type identifier.
        /// </value>
        public int? ClientIdentifierTypeID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pregnant.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pregnant; otherwise, <c>false</c>.
        /// </value>
        public bool IsPregnant { get; set; }

        /// <summary>
        /// Gets or sets preferred name.
        /// </summary>
        /// <value>
        /// The preferred named.
        /// </value>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether contact is deceased
        /// </summary>
        /// <value>
        /// <c>true</c> if this patient is deceased; otherwise, <c>false</c>.
        /// </value>
        public bool? IsDeceased { get; set; }

        /// <summary>
        /// Gets or sets the deceased date.
        /// </summary>
        /// <value>
        /// The deceased date.
        /// </value>
        public DateTime? DeceasedDate { get; set; }

        /// <summary>
        /// Gets or sets the Cause of death identifier.
        /// </summary>
        /// <value>
        /// The cause of death identifier.
        /// </value>
        public int? CauseOfDeath { get; set; }

        /// <summary>
        /// Gets or sets the contact method identifier.
        /// </summary>
        /// <value>
        /// The contact method identifier.
        /// </value>
        public int? ContactMethodID { get; set; }

        /// <summary>
        /// Gets or sets the referral source identifier.
        /// </summary>
        /// <value>
        /// The referral source identifier.
        /// </value>
        public int? ReferralSourceID { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the contact presenting problem.
        /// </summary>
        /// <value>
        /// The contact presenting problem.
        /// </value>
        public ContactPresentingProblemModel ContactPresentingProblem { get; set; }
    }
}
