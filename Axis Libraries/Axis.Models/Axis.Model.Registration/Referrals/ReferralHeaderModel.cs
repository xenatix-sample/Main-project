using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.Registration.Referral
{
    public class ReferralHeaderModel : BaseEntity
    {

        /// <summary>
        /// Gets or sets the referral header identifier.
        /// </summary>
        /// <value>
        /// The referral header identifier.
        /// </value>
        public long ReferralHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ParentContactID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the referral status identifier.
        /// </summary>
        /// <value>
        /// The referral status identifier.
        /// </value>
        public int? ReferralStatusID { get; set; }

        /// <summary>
        /// Gets or sets the referral type identifier.
        /// </summary>
        /// <value>
        /// The referral type identifier.
        /// </value>
        public int? ReferralTypeID { get; set; }

        /// <summary>
        /// Gets or sets the resource type identifier.
        /// </summary>
        /// <value>
        /// The resource type identifier.
        /// </value>
        public int? ResourceTypeID { get; set; }

        /// <summary>
        /// Gets or sets the referral category source identifier.
        /// </summary>
        /// <value>
        /// The referral category source identifier.
        /// </value>
        public int? ReferralCategorySourceID { get; set; }

        /// <summary>
        /// Gets or sets the referral source identifier.
        /// </summary>
        /// <value>
        /// The referral source identifier.
        /// </value>
        public int? ReferralSourceID { get; set; }

        /// <summary>
        /// Gets or sets the referral origin identifier.
        /// </summary>
        /// <value>
        /// The referral origin identifier.
        /// </value>
        public int? ReferralOriginID { get; set; }

        /// <summary>
        /// Gets or sets the program identifier.
        /// </summary>
        /// <value>
        /// The program identifier.
        /// </value>
        public long? OrganizationID { get; set; }


        /// <summary>
        /// Gets or sets the Referral Organization identifier.
        /// </summary>
        /// <value>
        /// The Referral Organization identifier.
        /// </value>
        public int? ReferralOrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the referral organization.
        /// </summary>
        /// <value>
        /// The referral organization.
        /// </value>
        public string OtherOrganization { get; set; }

        /// <summary>
        /// Gets or sets the referral date.
        /// </summary>
        public DateTime ReferralDate { get; set; }

        /// <summary>
        /// Gets or sets the is linked to contact.
        /// </summary>
        /// <value>
        /// The is linked to contact.
        /// </value>
        public bool? IsLinkedToContact { get; set; }

        /// <summary>
        /// Gets or sets the is referrer converted to contact.
        /// </summary>
        /// <value>
        /// The is referrer converted to contact.
        /// </value>
        public bool? IsReferrerConvertedToCollateral { get; set; }

        /// <summary>
        /// Gets or sets the referral other source identifier.
        /// </summary>
        /// <value>
        /// The referral other source identifier.
        /// </value>
        public string OtherSource { get; set; }

        /// <summary>
        /// Gets or sets the contact relation ship.
        /// </summary>
        /// <value>
        /// The contact relation ship.
        /// </value>
        public ContactRelationshipModel ContactRelationShip { get; set; }

        /// <summary>
        /// Gets or sets the living with client status.
        /// </summary>
        /// <value>
        /// The living with client status.
        /// </value>
        public bool? LivingWithClientStatus { get; set; }

        /// <summary>
        /// Gets or sets the driver license.
        /// </summary>
        /// <value>
        /// The driver license.
        /// </value>
        public string DriverLicense { get; set; }

        /// <summary>
        /// Gets or sets the driver license state identifier.
        /// </summary>
        /// <value>
        /// The driver license state identifier.
        /// </value>
        public int? DriverLicenseStateID { get; set; }
    }
}
