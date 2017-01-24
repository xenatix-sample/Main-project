using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.ECI.Models
{
    public class NoteHeaderViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteHeaderViewModel"/> class.
        /// </summary>
        public NoteHeaderViewModel()
        {
            Discharge = new DischargeViewModel();
            ProgressNote = new ProgressNoteViewModel();
            ProgressNoteAssessment = new ProgressNoteAssessmentViewModel();
        }

        /// <summary>
        /// Gets or sets the note header identifier.
        /// </summary>
        /// <value>
        /// The note header identifier.
        /// </value>
        public long NoteHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the note type identifier.
        /// </summary>
        /// <value>
        /// The note type identifier.
        /// </value>
        public int NoteTypeID { get; set; }

        /// <summary>
        /// Gets or sets the taken time.
        /// </summary>
        /// <value>
        /// The taken time.
        /// </value>
        public DateTime? TakenTime { get; set; }

        /// <summary>
        /// Gets or sets the taken by.
        /// </summary>
        /// <value>
        /// The taken by.
        /// </value>
        public int TakenBy { get; set; }

        /// <summary>
        /// Gets or sets the discharge.
        /// </summary>
        /// <value>
        /// The discharge.
        /// </value>
        public DischargeViewModel Discharge { get; set; }

        /// <summary>
        /// Gets or sets the progress note.
        /// </summary>
        /// <value>
        /// The progress note.
        /// </value>
        public ProgressNoteViewModel ProgressNote { get; set; }

        public ProgressNoteAssessmentViewModel ProgressNoteAssessment { get; set; }
    }
}
