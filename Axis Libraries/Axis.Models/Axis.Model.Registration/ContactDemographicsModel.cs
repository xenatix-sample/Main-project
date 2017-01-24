using System;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Represents contact demography details
    /// </summary>
    public class ContactDemographicsModel : ContactBaseModel
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
        /// 
        /// </summary>
        public string ProgramUnit { get; set; }
        
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
        /// Gets or sets the DriverLicense identifier.
        /// </summary>
        /// <value>
        /// The DL value.
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
        /// Gets or sets the is pregnant.
        /// </summary>
        /// <value>
        /// The is pregnant.
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
        /// Gets or sets the prefix.
        /// </summary>
        /// <value>
        /// The prefix.
        /// </value>
        public int? TitleID { get; set; }

        /// <summary>
        /// Gets or sets the contact gender text.
        /// </summary>
        /// <value>
        /// The contact gender text.
        /// </value>
        public string ContactGenderText { get; set; } //Added for contact search

        /// <summary>
        /// Gets or sets the client type text.
        /// </summary>
        /// <value>
        /// The client type text.
        /// </value>
        public string ClientTypeText { get; set; } //Added for contact search

        /// <summary>
        /// Gets or sets the color.
        /// </summary>
        /// <value>
        /// The color.
        /// </value>
        public string Color { get; set; } //Addedfor contact search

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; } //Added for contact search

        /// <summary>
        /// Gets or sets the mpi.
        /// </summary>
        /// <value>
        /// The mpi.
        /// </value>
        public string MPI { get; set; } //Addedfor contact search

        /// <summary>
        /// Gets or sets the full address.
        /// </summary>
        /// <value>
        /// The full address.
        /// </value>
        public string FullAddress { get; set; } //Added for contact search by suresh on 08-10-2015

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

        /// <summary>
        /// Gets or sets Suffix
        /// </summary>
        /// <value>
        /// Suffix
        /// </value>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the contact presenting problem.
        /// </summary>
        /// <value>
        /// The contact presenting problem.
        /// </value>
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
