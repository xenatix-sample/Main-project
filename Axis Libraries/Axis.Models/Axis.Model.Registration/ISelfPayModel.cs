using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Registration
{
    public interface ISelfPayModel
    {
        /// <summary>
        /// Gets or sets the self pay identifier.
        /// </summary>
        /// <value>
        /// The self pay identifier.
        /// </value>
        long SelfPayID { get; set; }

        /// <summary>
        /// Gets or sets the organisation detail identifier.
        /// </summary>
        /// <value>
        /// The organisation detail identifier.
        /// </value>
        long OrganizationDetailID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the amount.
        /// </summary>
        /// <value>
        /// The amount.
        /// </value>
        decimal? SelfPayAmount { get; set; }

        /// <summary>
        /// Gets or sets the type of the amount.
        /// </summary>
        /// <value>
        /// The type of the amount.
        /// </value>
        bool? IsPercent { get; set; }

        /// <summary>
        /// Gets or sets the effective date.
        /// </summary>
        /// <value>
        /// The effective date.
        /// </value>
        DateTime? EffectiveDate { get; set; }

        /// <summary>
        /// Gets or sets the expiration date.
        /// </summary>
        /// <value>
        /// The expiration date.
        /// </value>
        DateTime? ExpirationDate { get; set; }
    }
}
