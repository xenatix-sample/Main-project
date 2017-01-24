using System;

namespace Axis.Model.BusinessAdmin
{
    public class MergeContactModel
    {
        /// <summary>
        /// Merge Contact Mapping Id 
        /// </summary>
        /// <value>
        /// The Merge Contact Mapping identifier.
        /// </value>
        public long MergedContactsMappingID { get; set; }

        /// <summary>
        /// Contact Id of contact
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the MRN.
        /// </summary>
        /// <value>
        /// The MRN.
        /// </value>
        public long? MRN { get; set; }

        /// <summary>
        /// Gets or sets the First Name.
        /// <value>
        /// The First Name.
        /// </value>
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        /// Gets or sets the Last Name.
        /// </summary>
        /// <value>
        /// The Last Name.
        /// </value>
        public string LastName { get; set; }

        /// <summary>
        /// Gets or sets the dob.
        /// </summary>
        /// <value>
        /// The dob.
        /// </value>
        public DateTime? DOB { get; set; }

        /// <summary>
        /// Gets or sets the SSN.
        /// </summary>
        /// <value>
        /// The SSN.
        /// </value>
        public string SSN { get; set; }

        /// <summary>
        /// Gets or sets the DriverLicense identifier.
        /// </summary>
        /// <value>
        /// The DL value.
        /// </value>
        public string DriverLicense { get; set; }

        /// <summary>
        /// Gets or sets the StateProvinceName.
        /// </summary>
        /// <value>
        /// The StateProvinceName.
        /// </value>
        public string StateProvinceName { get; set; }

        /// <summary>
        /// Gets or sets the Email.
        /// </summary>
        /// <value>
        /// The Email.
        /// </value>
        public string Email { get; set; }

        /// <summary>
        /// Gets or sets the PhoneNumber.
        /// </summary>
        /// <value>
        /// The PhoneNumber.
        /// </value>
        public string PhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the merge date.
        /// </summary>
        /// <value>
        /// The merge date.
        /// </value>
        public DateTime? MergedDate { get; set; }

        /// <summary>
        /// Gets or sets the IsMerge.
        /// </summary>
        /// <value>
        /// The flag to check master.
        /// </value>
        public bool IsMaster { get; set; }
    }
}
