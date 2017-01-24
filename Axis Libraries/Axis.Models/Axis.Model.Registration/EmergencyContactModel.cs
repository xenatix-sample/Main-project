using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Model to hold Emergency Contact details
    /// </summary>
    public class EmergencyContactModel : ContactBaseModel
    {
        /// <summary>
        /// Contact id of Contact
        /// </summary>
        public long ParentContactID { get; set; }

        /// <summary>
        /// Contact relationship id
        /// </summary>
        public long ContactRelationshipID { get; set; }

        /// <summary>
        /// Relationship type id
        /// </summary>
        public int RelationshipTypeID { get; set; }
        
        /// <summary>
        /// Is Living with Client
        /// </summary>
        public int? LivingWithClientStatusID { get; set; }

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
        /// Gets or sets the alternate identifier.
        /// </summary>
        /// <value>
        /// The alternate identifier.
        /// </value>
        public string AlternateID { get; set; }
    }
}