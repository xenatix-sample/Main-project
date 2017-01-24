using Axis.Model.Address;
using System;

namespace Axis.Model.BusinessAdmin
{
    public class PlanAddressesModel : AddressModel
    {

        /// <summary>
        /// Gets or sets the payor Address Identifier.
        /// </summary>
        /// <value>
        /// The payor Address identifier.
        /// </value>
        public long PayorAddressID { get; set; }

        /// <summary>
        /// Gets or sets the payor plan identifier.
        /// </summary>
        /// <value>
        /// The payor plan identifier.
        /// </value>
        public int PayorPlanID { get; set; }

        /// <summary>
        /// Gets or sets the electronic payor identifier.
        /// </summary>
        /// <value>
        /// The electronic payor identifier.
        /// </value>
        public string ElectronicPayorID { get; set; }


        /// <summary>
        /// Gets or sets the Contact ID.
        /// </summary>
        /// <value>
        /// Contact ID.
        /// </value>
        public string ContactID { get; set; }

        /// <summary>
        /// Gets or sets the Effective Date.
        /// </summary>
        /// <value>
        /// Effective Date
        /// </value>
        public DateTime? EffectiveDate { get; set; }


        /// <summary>
        /// Gets or sets the Expiration Date.
        /// </summary>
        /// <value>
        /// Expiration Date
        /// </value>
        public DateTime? ExpirationDate { get; set; }


    }
}
