using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common.Lookups.PayorPlan
{
    public class PayorPlan :BaseEntity
    {
        /// <summary>
        /// Gets or sets the payor plan identifier.
        /// </summary>
        /// <value>The payor plan identifier.</value>
        public int PayorPlanID { get; set; }

        /// <summary>
        /// Gets or sets the payor plan identifier.
        /// </summary>
        /// <value>The payor plan identifier.</value>
        public int PayorID { get; set; }


        /// <summary>
        /// Gets or sets the plan identifier.
        /// </summary>
        /// <value>
        /// The plan identifier.
        /// </value>
        public string PlanID { get; set; }

        /// <summary>
        /// Gets or sets the name of the plan.
        /// </summary>
        /// <value>
        /// The name of the plan.
        /// </value>
        public string PlanName { get; set; }

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




    }
}
