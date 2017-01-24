using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class SchedulingOccurrenceViewModel : BaseViewModel
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