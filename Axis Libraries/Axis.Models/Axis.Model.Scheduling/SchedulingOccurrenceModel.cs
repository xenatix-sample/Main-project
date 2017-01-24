using Axis.Model.Common;

namespace Axis.Model.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class SchedulingOccurrenceModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the scheduling occurrence identifier.
        /// </summary>
        /// <value>
        /// The scheduling occurrence identifier.
        /// </value>
        public int SchedulingOccurrenceID { get; set; }

        /// <summary>
        /// Gets or sets the scheduling occurrence.
        /// </summary>
        /// <value>
        /// The scheduling occurrence.
        /// </value>
        public int SchedulingOccurrence { get; set; }
    }
}