using Axis.Model.Registration;
using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;

namespace Axis.Plugins.Registration.Models
{
    public class ContactBaseViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the ContactBaseViewModel.
        /// </summary>
        public ContactBaseViewModel()
        {
            Addresses = new List<ContactAddressModel>();
            Emails = new List<ContactEmailModel>();
            Phones = new List<ContactPhoneModel>();
            ClientAlternateIDs = new List<ContactClientIdentifierModel>();
        }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

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
        /// Gets or sets the suffix.
        /// </summary>
        /// <value>
        /// The suffix.
        /// </value>
        public int? SuffixID { get; set; }

        /// <summary>
        /// Gets or sets the gender identifier.
        /// </summary>
        /// <value>
        /// The gender identifier.
        /// </value>
        public int? GenderID { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the age.
        /// </summary>
        /// <value>
        /// The age.
        /// </value>
        public int? Age { get; set; }

        public decimal? GestationalAge { get; set; }

        /// <summary>
        /// Gets or sets the addresses.
        /// </summary>
        /// <value>
        /// The addresses.
        /// </value>
        public List<ContactAddressModel> Addresses { get; set; }

        /// <summary>
        /// Gets or sets the emails.
        /// </summary>
        /// <value>
        /// The emails.
        /// </value>
        public List<ContactEmailModel> Emails { get; set; }

        /// <summary>
        /// Gets or sets the phones.
        /// </summary>
        /// <value>
        /// The phones.
        /// </value>
        public List<ContactPhoneModel> Phones { get; set; }

        /// <summary>
        /// Gets or sets the Client Alternate IDs.
        /// </summary>
        /// <value>
        /// true/false
        /// </value>
        public List<ContactClientIdentifierModel> ClientAlternateIDs { get; set; }

        /// <summary>
        /// Gets or sets a value isContactNotDirty.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is contact not dirty; otherwise, <c>false</c>.
        /// </value>
        public bool isContactNotDirty { get; set; }
    }
}

