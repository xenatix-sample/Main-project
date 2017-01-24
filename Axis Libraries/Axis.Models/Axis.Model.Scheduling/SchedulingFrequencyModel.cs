using Axis.Model.Common;
namespace Axis.Model.Scheduling
{
    /// <summary>
    ///
    /// </summary>
    public class SchedulingFrequencyModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the scheduling frequency identifier.
        /// </summary>
        /// <value>
        /// The scheduling frequency identifier.
        /// </value>
        public int SchedulingFrequencyID { get; set; }

        /// <summary>
        /// Gets or sets the scheduling frequency.
        /// </summary>
        /// <value>
        /// The scheduling frequency.
        /// </value>
        public string SchedulingFrequency { get; set; }
    }
}