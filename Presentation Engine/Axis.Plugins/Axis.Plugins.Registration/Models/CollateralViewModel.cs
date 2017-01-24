using Axis.Plugins.Registration.Models;
using System;

namespace Axis.Model.Registration.Model
{
    /// <summary>
    /// Model to hold Collateral details
    /// </summary>
    /// <seealso cref="Axis.Plugins.Registration.Models.ContactBaseViewModel" />
    public class CollateralViewModel : ContactBaseViewModel
    {
        /// <summary>
        /// Contact Id of contact
        /// </summary>
        /// <value>
        /// The parent contact identifier.
        /// </value>
        public long ParentContactID { get; set; }

        /// <summary>
        /// Contact relationship id
        /// </summary>
        /// <value>
        /// The contact relationship identifier.
        /// </value>
        public long ContactRelationshipID { get; set; }

        /// <summary>
        /// Relationship type id
        /// </summary>
        /// <value>
        /// The relationship type identifier.
        /// </value>
        public int RelationshipTypeID { get; set; }

        /// <summary>
        /// Is Living with Client
        /// </summary>
        /// <value>
        /// The is living with client.
        /// </value>
        public bool? LivingWithClientStatus { get; set; }

        /// <summary>
        /// Receive Correspondence
        /// </summary>
        /// <value>
        /// Receive Correspondence
        /// </value>
        public int? ReceiveCorrespondenceID { get; set; }

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
        /// Gets or sets the driver license state identifier.
        /// </summary>
        /// <value>
        /// The driver license state identifier.
        /// </value>
        public int? DriverLicenseStateID { get; set; }

        /// <summary>
        /// Gets or sets the alternate identifier.
        /// </summary>
        /// <value>
        /// The alternate identifier.
        /// </value>
        public string AlternateID { get; set; }

        /// <summary>
        /// Gets or sets the client identifier type identifier.
        /// </summary>
        /// <value>
        /// The client identifier type identifier.
        /// </value>
        public int? ClientIdentifierTypeID { get; set; }

        /// <summary>
        /// Gets or sets the emergency contact.
        /// </summary>
        /// <value>
        /// The emergency contact.
        /// </value>
        public bool? EmergencyContact { get; set; }

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
        /// Gets or sets the employment status identifier.
        /// </summary>
        /// <value>
        /// The employment status identifier.
        /// </value>
        public int? EmploymentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the Copy Contact Address.
        /// </summary>
        /// <value>
        /// true/false
        /// </value>
        public bool CopyContactAddress { get; set; }

        /// <summary>
        /// Gets or sets the veteran status identifier.
        /// </summary>
        /// <value>
        /// The veteran status identifier.
        /// </value>
        public int? VeteranStatusID { get; set; }

        /// <summary>
        /// Gets or sets the relationship group identifier.
        /// </summary>
        /// <value>
        /// The relationship group identifier.
        /// </value>
        public long? RelationshipGroupID { get; set; }

        /// <summary>
        /// Gets or sets the other relationship.
        /// </summary>
        /// <value>
        /// The other relationship.
        /// </value>
        public string OtherRelationship { get; set; }

        /// <summary>
        /// Gets or sets the Policy Holder.
        /// </summary>
        /// <value>
        /// The Policy Holder.
        /// </value>
        public bool? IsPolicyHolder { get; set; }

        /// <summary>
        /// Gets or sets the collateral effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        public DateTime? CollateralEffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the collateral expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? CollateralExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the relationships.
        /// </summary>
        /// <value>
        /// The relationships.
        /// </value>
        public string Relationships { get; set; }

        /// <summary>
        /// Gets or sets the collateral types.
        /// </summary>
        /// <value>
        /// The collateral types.
        /// </value>
        public string CollateralTypes { get; set; }
    }
}
