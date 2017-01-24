using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.Registration.Model
{
    /// <summary>
    ///
    /// </summary>
    public class AdditionalDemographicsViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the additional demographic identifier.
        /// </summary>
        /// <value>
        /// The additional demographic identifier.
        /// </value>
        public long AdditionalDemographicID { get; set; }

        /// <summary>
        /// Gets or sets a value advance directvie.
        /// </summary>
        /// <value>
        ///  true false
        /// </value>
        public bool? AdvancedDirective { get; set; }

        /// <summary>
        /// Gets or sets the smoking status identifier.
        /// </summary>
        /// <value>
        /// The smoking status identifier.
        /// </value>
        public int? SmokingStatusID { get; set; }

        /// <summary>
        /// Gets or sets the other race.
        /// </summary>
        /// <value>
        /// The other race.
        /// </value>
        public string OtherRace { get; set; }

        /// <summary>
        /// Gets or sets the other ethnicity.
        /// </summary>
        /// <value>
        /// The other ethnicity.
        /// </value>
        public string OtherEthnicity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [looking for work].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [looking for work]; otherwise, <c>false</c>.
        /// </value>
        public bool? LookingForWork { get; set; }

        /// <summary>
        /// Gets or sets the school district identifier.
        /// </summary>
        /// <value>
        /// The school district identifier.
        /// </value>
        public int? SchoolDistrictID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the ethnicity identifier.
        /// </summary>
        /// <value>
        /// The ethnicity identifier.
        /// </value>
        public int? EthnicityID { get; set; }

        /// <summary>
        /// Gets or sets the primary language identifier.
        /// </summary>
        /// <value>
        /// The primary language identifier.
        /// </value>
        public int? PrimaryLanguageID { get; set; }

        /// <summary>
        /// Gets or sets the secondary language identifier.
        /// </summary>
        /// <value>
        /// The secondary language identifier.
        /// </value>
        public int? SecondaryLanguageID { get; set; }

        /// <summary>
        /// Gets or sets the legal status identifier.
        /// </summary>
        /// <value>
        /// The legal status identifier.
        /// </value>
        public int? LegalStatusID { get; set; }

        /// <summary>
        /// Gets or sets the living arrangement identifier.
        /// </summary>
        /// <value>
        /// The living arrangement identifier.
        /// </value>
        public int? LivingArrangementID { get; set; }

        /// <summary>
        /// Gets or sets the citizenship identifier.
        /// </summary>
        /// <value>
        /// The citizenship identifier.
        /// </value>
        public int? CitizenshipID { get; set; }

        /// <summary>
        /// Gets or sets the marital status identifier.
        /// </summary>
        /// <value>
        /// The marital status identifier.
        /// </value>
        public int? MaritalStatusID { get; set; }

        /// <summary>
        /// Gets or sets the employment status identifier.
        /// </summary>
        /// <value>
        /// The employment status identifier.
        /// </value>
        public int? EmploymentStatusID { get; set; }

        public string PlaceOfEmployment { get; set; }

        public DateTime? EmploymentBeginDate { get; set; }

        public DateTime? EmploymentEndDate { get; set; }

        /// <summary>
        /// Gets or sets the religion identifier.
        /// </summary>
        /// <value>
        /// The religion identifier.
        /// </value>
        public int? ReligionID { get; set; }

        /// <summary>
        /// Gets or sets the veteran status identifier.
        /// </summary>
        /// <value>
        /// The veteran status identifier.
        /// </value>
        public int? VeteranStatusID { get; set; }

        /// <summary>
        /// Gets or sets the education status identifier.
        /// </summary>
        /// <value>
        /// The education status identifier.
        /// </value>
        public int? EducationStatusID { get; set; }

        /// <summary>
        /// Gets or sets the school attended.
        /// </summary>
        /// <value>
        /// The school attended.
        /// </value>
        public string SchoolAttended { get; set; }

        /// <summary>
        /// Gets or sets the school begin date.
        /// </summary>
        /// <value>
        /// The school begin date.
        /// </value>
        public DateTime? SchoolBeginDate { get; set; }

        /// <summary>
        /// Gets or sets the school end date.
        /// </summary>
        /// <value>
        /// The school end date.
        /// </value>
        public DateTime? SchoolEndDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [living will].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [living will]; otherwise, <c>false</c>.
        /// </value>
        public bool? LivingWill { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [power of attorney].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [power of attorney]; otherwise, <c>false</c>.
        /// </value>
        public bool? PowerOfAttorney { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [InterpreterRequired].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [InterpreterRequired]; otherwise, <c>false</c>.
        /// </value>
        public bool InterpreterRequired { get; set; }

        /// <summary>
        /// Gets or sets the other legalstatus.
        /// </summary>
        /// <value>
        /// The other legalstatus.
        /// </value>
        public string OtherLegalstatus { get; set; }

        /// <summary>
        /// Gets or sets the other preferred language.
        /// </summary>
        /// <value>
        /// The other preferred language.
        /// </value>
        public string OtherPreferredLanguage { get; set; }

        /// <summary>
        /// Gets or sets the other secondary language.
        /// </summary>
        /// <value>
        /// The other secondary language.
        /// </value>
        public string OtherSecondaryLanguage { get; set; }

        /// <summary>
        /// Gets or sets the other citizenship.
        /// </summary>
        /// <value>
        /// The other citizenship.
        /// </value>
        public string OtherCitizenship { get; set; }

        /// <summary>
        /// Gets or sets the other education.
        /// </summary>
        /// <value>
        /// The other education.
        /// </value>
        public string OtherEducation { get; set; }

        /// <summary>
        /// Gets or sets the other living arrangement.
        /// </summary>
        /// <value>
        /// The other living arrangement.
        /// </value>
        public string OtherLivingArrangement { get; set; }

        /// <summary>
        /// Gets or sets the other veteranstatus.
        /// </summary>
        /// <value>
        /// The other veteranstatus.
        /// </value>
        public string OtherVeteranStatus { get; set; }

        /// <summary>
        /// Gets or sets the other employment status.
        /// </summary>
        /// <value>
        /// The other employment status.
        /// </value>
        public string OtherEmploymentStatus { get; set; }

        /// <summary>
        /// Gets or sets the other religion.
        /// </summary>
        /// <value>
        /// The other religion.
        /// </value>
        public string OtherReligion { get; set; }

        /// <summary>
        /// Gets or sets the directive type.
        /// </summary>
        /// <value>
        /// The Directive type.
        /// </value>
        public int? AdvancedDirectiveTypeID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether GenerateMRN.
        /// </summary>
        /// <value>
        ///   <c>true</c> if GenerateMRN otherwise, <c>false</c>.
        /// </value>
        public bool? GenerateMRN { get; set; }
    }
}