using Axis.Model.Address;
using Axis.Model.Common;
using System;
using System.Collections.Generic;

namespace Axis.Model.BusinessAdmin
{
    public class PayorsModel : BaseEntity
    {
        public int PayorID { get; set; }


        /// <summary>
        /// Gets or sets the name of the payor.
        /// </summary>
        /// <value>The name of the payor.</value>
        public string PayorName { get; set; }

        /// <summary>
        /// Gets or sets the payor code.
        /// </summary>
        /// <value>The payor code.</value>
        public int PayorCode { get; set; }

        /// <summary>
        /// Gets or sets the Payor Type.
        /// </summary>
        /// <value>
        /// The payor type.
        /// </value>
        public int? PayorTypeID { get; set; }


        /// <summary>
        /// Gets or sets the payor effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        public DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the payor expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public DateTime? ExpirationDate { get; set; }


        /// <summary>
        /// Gets or sets the payor expiration reason.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        public string ExpirationReason { get; set; }


    }
}
