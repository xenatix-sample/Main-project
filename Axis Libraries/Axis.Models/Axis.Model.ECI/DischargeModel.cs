using Axis.Model.Common;
using System;

namespace Axis.Model.ECI
{
    public class DischargeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the discharge identifier.
        /// </summary>
        /// <value>
        /// The discharge identifier.
        /// </value>
        public long DischargeID { get; set; }
        /// <summary>
        /// Gets or sets the progress note identifier.
        /// </summary>
        /// <value>
        /// The progress note identifier.
        /// </value>
        public long ProgressNoteID { get; set; }
        /// <summary>
        /// Gets or sets the discharge type identifier.
        /// </summary>
        /// <value>
        /// The discharge type identifier.
        /// </value>
        public int? DischargeTypeID { get; set; }
        /// <summary>
        /// Gets or sets the taken by.
        /// </summary>
        /// <value>
        /// The taken by.
        /// </value>
        public int? TakenBy { get; set; }
        /// <summary>
        /// Gets or sets the discharge date.
        /// </summary>
        /// <value>
        /// The discharge date.
        /// </value>
        public DateTime? DischargeDate { get; set; }
        /// <summary>
        /// Gets or sets the discharge reason identifier.
        /// </summary>
        /// <value>
        /// The discharge reason identifier.
        /// </value>
        public int? DischargeReasonID { get; set; }
    }
}
