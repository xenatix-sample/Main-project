using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class SchedulingFrequencyViewModel : BaseViewModel
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