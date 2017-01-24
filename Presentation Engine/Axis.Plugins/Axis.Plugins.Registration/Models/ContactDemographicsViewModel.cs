using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System;

namespace Axis.Plugins.Registration.Model
{
    /// <summary>
    /// Represents contact demography view model
    /// </summary>
    public class ContactDemographicsViewModel : ContactBaseViewModel
    {
        /// <summary>
        /// Gets or sets the preferred gender identifier.
        /// </summary>
        /// <value>
        /// The preferred gender identifier.
        /// </value>
        public int? PreferredGenderID { get; set; }

        /// <summary>
        /// Gets or sets the client type identifier.
        /// </summary>
        /// <value>
        /// The client type identifier.
        /// </value>
        public int? ClientTypeID { get; set; }

        /// <summary>
        /// Gets or sets the program unit.
        /// </summary>
        /// <value>
        /// The program unit.
        /// </value>
        public string ProgramUnit { get; set; }

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
        /// Gets or sets the DriverLicense #.
        /// </summary>
        /// <value>
        /// Driver License #.
        /// </value>
        public string DriverLicense { get; set; }

        /// <summary>
        /// Gets or sets the driver license state identifier.
        /// </summary>
        /// <value>
        /// The driver license state identifier.
        /// </value>
        public int? DriverLicenseStateID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pregnant.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pregnant; otherwise, <c>false</c>.
        /// </value>
        public bool? IsPregnant { get; set; }

        /// <summary>
        /// Gets or sets preferred name.
        /// </summary>
        /// <value>
        /// The preferred name.
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
        /// Gets or sets ReportingUnit
        /// </summary>
        /// <value>
        /// ReportingUnit
        /// </value>
        public string ReportingUnit { get; set; }

        /// <summary>
        /// Gets or sets ServiceCoordinator
        /// </summary>
        /// <value>
        /// ServiceCoordinator
        /// </value>
        public string ServiceCoordinator { get; set; }

        /// <summary>
        /// Gets or sets ServiceCoordinatorPhone
        /// </summary>
        /// <value>
        /// ServiceCoordinatorPhone
        /// </value>
        public string ServiceCoordinatorPhone { get; set; }

        /// <summary>
        /// Gets or sets AdjustedAge
        /// </summary>
        /// <value>
        /// AdjustedAge
        /// </value>
        public string AdjustedAge { get; set; }

        /// <summary>
        /// Gets or sets DispositionStatus
        /// </summary>
        /// <value>
        /// DispositionStatus
        /// </value>
        public string DispositionStatus { get; set; }

        public ContactPresentingProblemModel ContactPresentingProblem { get; set; }

        /// <summary>
        /// Boolean flag if this contact is merged with other contact
        /// </summary>
        public bool IsMerged { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MergedMRN { get; set; }
    }
}
