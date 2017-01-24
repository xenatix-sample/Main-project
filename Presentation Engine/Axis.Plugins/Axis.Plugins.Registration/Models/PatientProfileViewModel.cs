using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Model
{
    /// <summary>
    /// View model for Patient Profile. To use in UI.
    /// </summary>
    public class PatientProfileViewModel : ContactBaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PatientProfileViewModel"/> class.
        /// </summary>
        public PatientProfileViewModel()
        {           
        }

        /// <summary>
        /// Gets or sets preferred name.
        /// </summary>
        /// <value>
        /// The preferred name.
        /// </value>
        public string PreferredName { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the CareID identifier.
        /// </summary>
        /// <value>
        /// The CareID identifier.
        /// </value>
        public int? CareID { get; set; }

        /// <summary>
        /// Gets or sets the ClientTypeID identifier.
        /// </summary>
        /// <value>
        /// The ClientTypeID identifier.
        /// </value>
        public int? ClientTypeID { get; set; }

        /// <summary>
        /// The patient's legal status for the header
        /// </summary>
        public int? LegalStatusID { get; set; }

        /// <summary>
        /// The emergency contact's first name
        /// </summary>
        public string EmergencyContactFirstName { get; set; }

        /// <summary>
        /// The emergency contact's last name
        /// </summary>
        public string EmergencyContactLastName { get; set; }

        /// <summary>
        /// The emergency contact's phone number
        /// </summary>
        public string EmergencyContactPhoneNumber{ get; set; }

        /// <summary>
        /// The emergency contact's phone number extension
        /// </summary>
        public string EmergencyContactExtension { get; set; }

        public int? PreferredGenderID { get; set; }
    }
}