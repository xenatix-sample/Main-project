using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common.Lookups.PayorPlanGroup
{
    public class PayorPlanGroup : BaseEntity
    {

        /// <summary>
        /// Gets or sets the payor group identifier.
        /// </summary>
        /// <value>The payor group identifier.</value>
        public int PayorGroupID { get; set; }

        /// <summary>
        /// Gets or sets the payor plan identifier.
        /// </summary>
        /// <value>The payor plan identifier.</value>
        public int PayorPlanID { get; set; }

        /// <summary>
        /// Gets or sets the group identifier.
        /// </summary>
        /// <value>The group identifier.</value>
        public string GroupID { get; set; }

        /// <summary>
        /// Gets or sets the name of the group.
        /// </summary>
        /// <value>The name of the group.</value>
        public string GroupName { get; set; }

    }
}
