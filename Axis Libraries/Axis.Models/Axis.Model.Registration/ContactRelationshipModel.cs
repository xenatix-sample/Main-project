using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ContactRelationshipModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the contact Relationship identifier.
        /// </summary>
        /// <value>
        /// The contact Relationship identifier.
        /// </value>
        public long ContactRelationshipTypeID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the ParentContactID identifier.
        /// </summary>
        /// <value>
        /// The ParentContactID identifier.
        /// </value>
        public long ParentContactID { get; set; }

        /// <summary>
        /// Gets or sets the RelationshipGroupID identifier.
        /// </summary>
        /// <value>
        /// The RelationshipGroupID identifier.
        /// </value>
        public long RelationshipGroupID { get; set; }

        /// <summary>
        /// Gets or sets the RelationshipTypeID identifier.
        /// </summary>
        /// <value>
        /// The RelationshipTypeID identifier.
        /// </value>
        public int RelationshipTypeID { get; set; }

        /// <summary>
        /// Gets or sets the IsPolicyHolder identifier.
        /// </summary>
        /// <value>
        /// The IsPolicyHolder identifier.
        /// </value>
        public bool IsPolicyHolder { get; set; }

        /// <summary>
        /// Gets or sets the OtherRelationship identifier.
        /// </summary>
        /// <value>
        /// The OtherRelationship identifier.
        /// </value>
        public string OtherRelationship { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        /// Gets or sets the living with client status.
        /// </summary>
        /// <value>
        /// The living with client status.
        /// </value>
        public bool? LivingWithClientStatus { get; set; }

        /// <summary>
        /// Gets or sets the is collateral.
        /// </summary>
        /// <value>
        /// The is collateral.
        /// </value>
        public bool? IsCollateral { get; set; }

    }
}