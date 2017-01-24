using Axis.Helpers.Validation;
using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.ECI
{
    /// <summary>
    /// Class ECIAdditionalDemographicsModel.
    /// </summary>
    public class ECIAdditionalDemographicsModel : BaseEntity
    {

        /// <summary>
        /// Gets or sets the additional demographic identifier.
        /// </summary>
        /// <value>The additional demographic identifier.</value>
        public long AdditionalDemographicID { get; set; }

        /// <summary>
        /// Gets or sets the registration additional demographic identifier.
        /// </summary>
        /// <value>The registration additional demographic identifier.</value>
        public long? RegistrationAdditionalDemographicID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>The contact identifier.</value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the referral disposition status identifier.
        /// </summary>
        /// <value>The referral disposition status identifier.</value>
        public int? ReferralDispositionStatusID { get; set; }

        /// <summary>
        /// Gets or sets the school district identifier.
        /// </summary>
        /// <value>The school district identifier.</value>
        public int? SchoolDistrictID { get; set; }

        /// <summary>
        /// Gets or sets the ethnicity identifier.
        /// </summary>
        /// <value>The ethnicity identifier.</value>
        public int? EthnicityID { get; set; }

        /// <summary>
        /// Gets or sets the language identifier.
        /// </summary>
        /// <value>The language identifier.</value>
        public int? LanguageID { get; set; }


        /// <summary>
        /// Gets or sets the other race.
        /// </summary>
        /// <value>The other race.</value>
        public string OtherRace { get; set; }

        /// <summary>
        /// Gets or sets the other ethnicity.
        /// </summary>
        /// <value>The other ethnicity.</value>
        public string OtherEthnicity { get; set; }
        
        /// <summary>
        /// Gets or sets the other preferred language.
        /// </summary>
        /// <value>The other preferred language.</value>
        public string OtherPreferredLanguage { get; set; }
        

        /// <summary>
        /// Gets or sets a value indicating whether this instance is interpreter required.
        /// </summary>
        /// <value><c>true</c> if this instance is interpreter required; otherwise, <c>false</c>.</value>
        public bool InterpreterRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is CPS involved.
        /// </summary>
        /// <value><c>null</c> if [is CPS involved] contains no value, <c>true</c> if [is CPS involved]; otherwise, <c>false</c>.</value>
        public bool? IsCPSInvolved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is child hospitalized.
        /// </summary>
        /// <value><c>null</c> if [is child hospitalized] contains no value, <c>true</c> if [is child hospitalized]; otherwise, <c>false</c>.</value>
        public bool? IsChildHospitalized { get; set; }

        /// <summary>
        /// Gets or sets the expected hospital discharge date.
        /// </summary>
        /// <value>The expected hospital discharge date.</value>
        public DateTime? ExpectedHospitalDischargeDate { get; set; }

        /// <summary>
        /// Gets or sets the birth weight LBS.
        /// </summary>
        /// <value>The birth weight LBS.</value>
        public short? BirthWeightLbs { get; set; }

        /// <summary>
        /// Gets or sets the birth weight oz.
        /// </summary>
        /// <value>The birth weight oz.</value>
        public short? BirthWeightOz { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is transfer.
        /// </summary>
        /// <value><c>null</c> if [is transfer] contains no value, <c>true</c> if [is transfer]; otherwise, <c>false</c>.</value>
        public bool? IsTransfer{get;set;}

        /// <summary>
        /// Gets or sets the transfer from.
        /// </summary>
        /// <value>The transfer from.</value>
        public string TransferFrom { get; set; }

        /// <summary>
        /// Gets or sets the transfer date.
        /// </summary>
        /// <value>The transfer date.</value>
        public DateTime? TransferDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is out of service area.
        /// </summary>
        /// <value><c>null</c> if [is out of service area] contains no value, <c>true</c> if [is out of service area]; otherwise, <c>false</c>.</value>
        public bool? IsOutOfServiceArea { get; set; }

        /// <summary>
        /// Gets or sets the reporting unit identifier.
        /// </summary>
        /// <value>The reporting unit identifier.</value>
        public int? ReportingUnitID { get; set; }

        /// <summary>
        /// Gets or sets the service coordinator identifier.
        /// </summary>
        /// <value>The service coordinator identifier.</value>
        public int? ServiceCoordinatorID { get; set; }

        /// <summary>
        /// Gets or sets the service coordinator phone identifier.
        /// </summary>
        /// <value>The service coordinator phone identifier.</value>
        public long? ServiceCoordinatorPhoneID { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }
    }
}
