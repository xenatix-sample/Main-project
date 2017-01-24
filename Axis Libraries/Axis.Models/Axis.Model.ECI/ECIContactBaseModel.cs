using System;
using System.Collections.Generic;
using Axis.Model.Common;
using Axis.Model.Registration;

namespace Axis.Model.ECI
{
    public class ECIContactBaseModel : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the ContactBaseModel.
        /// </summary>
        public ECIContactBaseModel()
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
        /// Gets or sets the Copy Contact Address.
        /// </summary>
        /// <value>
        /// true/false
        /// </value>
        public bool CopyContactAddress { get; set; }

        /// <summary>
        /// Gets or sets the Client Alternate IDs.
        /// </summary>
        /// <value>
        /// true/false
        /// </value>
        public List<ContactClientIdentifierModel> ClientAlternateIDs { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is contact not dirty.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is contact not dirty; otherwise, <c>false</c>.
        /// </value>
        public bool isContactNotDirty { get; set; }
    }
}
