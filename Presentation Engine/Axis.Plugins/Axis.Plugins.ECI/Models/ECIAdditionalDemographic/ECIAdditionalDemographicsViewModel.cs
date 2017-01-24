using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Axis.Plugins.ECI.Model;

namespace Axis.Plugins.ECI
{
    public class ECIAdditionalDemographicsViewModel : ECIBaseViewModel
    {
        /// <summary>
        /// Gets or sets the additional demographic identifier.
        /// </summary>
        /// <value>
        /// The additional demographic identifier.
        /// </value>
        public long AdditionalDemographicID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public new long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the disposition status Id.
        /// </summary>
        /// <value>
        /// The smoking status identifier.
        /// </value>
        public int? ReferralDispositionStatusID { get; set; }

        /// <summary>
        /// Gets or sets the school district identifier.
        /// </summary>
        /// <value>
        /// The school district identifier.
        /// </value>
        public int? SchoolDistrictID { get; set; }

        /// <summary>
        /// Gets or sets the ethnicity identifier.
        /// </summary>
        /// <value>
        /// The ethnicity identifier.
        /// </value>
        public int? EthnicityID { get; set; }

        /// <summary>
        /// Gets or sets the other ethnicity.
        /// </summary>
        /// <value>
        /// The other ethnicity.
        /// </value>
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
        /// Gets or sets a value indicating whether [InterpreterRequired].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [InterpreterRequired]; otherwise, <c>false</c>.
        /// </value>
        public bool InterpreterRequired { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IsCPSInvolved].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [IsCPSInvolved]; otherwise, <c>false</c>.
        /// </value>
        public bool? IsCPSInvolved { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IschildCurrentlyHospitalized].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [IschildCurrentlyHospitalized]; otherwise, <c>false</c>.
        /// </value>
        public bool? IsChildHospitalized { get; set; }

        /// <summary>
        /// Gets or sets the expected discharge date.
        /// </summary>
        /// <value>
        /// The expected discharge date.
        /// </value>
        public DateTime? ExpectedHospitalDischargeDate { get; set; }

        /// <summary>
        /// Gets or sets the birth weightLbs.
        /// </summary>
        /// <value>
        /// The birth weightLbs.
        /// </value>
        public short? BirthWeightLbs { get; set; }

        /// <summary>
        /// Gets or sets the birth weight Oz.
        /// </summary>
        /// <value>
        /// The birth weight Oz.
        /// </value>
        public short? BirthWeightOz { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IsTransfer].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [IsTransfer]; otherwise, <c>false</c>.
        /// </value>
        public bool? IsTransfer { get; set; }

        /// <summary>
        /// Gets or sets the ECI program transferred from.
        /// </summary>
        /// <value>
        /// The transfered in date.
        /// </value>
        public string TransferFrom { get; set; }

        /// <summary>
        /// Gets or sets the transfered in date.
        /// </summary>
        /// <value>
        /// The transfered in date.
        /// </value>
        public DateTime? TransferDate { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [IsOutOfServiceArea].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [IsOutOfServiceArea]; otherwise, <c>false</c>.
        /// </value>
        public bool? IsOutOfServiceArea { get; set; }

        /// <summary>
        /// Gets or sets the reporting unit identifier.
        /// </summary>
        /// <value>
        /// The reporting unit identifier.
        /// </value>
        public int? ReportingUnitID { get; set; }

        /// <summary>
        /// Gets or sets the  service coordinator.
        /// </summary>
        /// <value>
        /// The service coordinator.
        /// </value>
        /// 
        public int? ServiceCoordinatorID { get; set; }

        /// <summary>
        /// Gets or sets the phone.
        /// </summary>
        /// <value>
        /// The phone.
        /// </value>
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
