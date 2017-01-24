using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Clinical.Models
{
    public class NoteDetailsViewModel : ClinicalBaseViewModel
    {
        /// <summary>
        /// NoteID
        /// </summary>
        public long NoteID { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }
    }
}
