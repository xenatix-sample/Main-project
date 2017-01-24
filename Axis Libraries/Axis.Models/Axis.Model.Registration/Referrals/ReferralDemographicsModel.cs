using Axis.Model.Common;

namespace Axis.Model.Registration.Referral
{

    public class ReferralDemographicsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the referral identifier.
        /// </summary>
        /// <value>
        /// The referral identifier.
        /// </value>
        public long ReferralID { get; set; }

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
        /// Gets or sets the contact type identifier.
        /// </summary>
        /// <value>
        /// The contact type identifier.
        /// </value>
        public int? ContactTypeID { get; set; }

        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the middle.
        /// </summary>
        /// <value>
        /// The middle.
        /// </value>
        public string Middle { get; set; }

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the suffix identifier.
        /// </summary>
        /// <value>
        /// The suffix identifier.
        /// </value>
        public int? SuffixID { get; set; }

        /// <summary>
        /// Gets or sets the title identifier.
        /// </summary>
        /// <value>
        /// The title identifier.
        /// </value>
        public int? TitleID { get; set; }

        /// <summary>
        /// Gets or sets the preferred contact method identifier.
        /// </summary>
        /// <value>
        /// The preferred contact method identifier.
        /// </value>
        public int? PreferredContactMethodID { get; set; }

        /// <summary>
        /// Gets or sets the gestational age.
        /// </summary>
        /// <value>
        /// The gestational age.
        /// </value>
        public int? GestationalAge { get; set; }

        /// <summary>
        /// Gets or sets the name of the organization.
        /// </summary>
        /// <value>
        /// The name of the organization.
        /// </value>
        public string OrganizationName { get; set; }

    }
}
