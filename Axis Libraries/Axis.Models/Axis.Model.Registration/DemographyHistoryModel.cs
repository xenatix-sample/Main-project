using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    public class DemographyHistoryModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the transaction log identifier.
        /// </summary>
        /// <value>
        /// The transaction log identifier.
        /// </value>
        public long? TransactionLogID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int? UserID { get; set; }

        /// <summary>
        /// Gets or sets the first name of the user.
        /// </summary>
        /// <value>
        /// The first name of the user.
        /// </value>
        public string UserFirstName { get; set; }

        /// <summary>
        /// Gets or sets the last name of the user.
        /// </summary>
        /// <value>
        /// The last name of the user.
        /// </value>
        public string UserLastName { get; set; }

        /// <summary>
        /// Gets or sets the changed date.
        /// </summary>
        /// <value>
        /// The changed date.
        /// </value>
        public DateTime? ChangedDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the presenting problem.
        /// </summary>
        /// <value>
        /// The type of the presenting problem.
        /// </value>
        public string PresentingProblemType { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the mpi.
        /// </summary>
        /// <value>
        /// The mpi.
        /// </value>
        public string MPI { get; set; }

        /// <summary>
        /// Gets or sets the type of the client.
        /// </summary>
        /// <value>
        /// The type of the client.
        /// </value>
        public string ClientType { get; set; }

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
        /// Gets or sets the middle.
        /// </summary>
        /// <value>
        /// The middle.
        /// </value>
        public string Middle { get; set; }

        /// <summary>
        /// Gets or sets the name of the preferred.
        /// </summary>
        /// <value>
        /// The name of the preferred.
        /// </value>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public string Suffix { get; set; }

        /// <summary>
        /// Gets or sets the gender.
        /// </summary>
        /// <value>
        /// The gender.
        /// </value>
        public string Gender { get; set; }

        /// <summary>
        /// Gets or sets the preferred gender.
        /// </summary>
        /// <value>
        /// The preferred gender.
        /// </value>
        public string PreferredGender { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the dob status.
        /// </summary>
        /// <value>
        /// The dob status.
        /// </value>
        public string DOBStatus { get; set; }

        /// <summary>
        /// </summary>
        /// <value>
        /// The SSN.
        /// </value>
        public string SSN { get; set; }

        /// <summary>
        /// Gets or sets the SSN status.
        /// </summary>
        /// <value>
        /// The SSN status.
        /// </value>
        public string SSNStatus { get; set; }

        /// <summary>
        /// Gets or sets the driver license.
        /// </summary>
        /// <value>
        /// The driver license.
        /// </value>
        public string DriverLicense { get; set; }

        /// <summary>
        /// Gets or sets the state of the driver license.
        /// </summary>
        /// <value>
        /// The state of the driver license.
        /// </value>
        public string DriverLicenseState { get; set; }

        /// <summary>
        /// Gets or sets the preferred contact method.
        /// </summary>
        /// <value>
        /// The preferred contact method.
        /// </value>
        public string PreferredContactMethod { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is pregnant.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is pregnant; otherwise, <c>false</c>.
        /// </value>
        public bool? IsPregnant { get; set; }
    }
}
