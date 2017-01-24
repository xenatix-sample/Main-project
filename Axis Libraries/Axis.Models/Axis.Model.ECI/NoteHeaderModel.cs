using Axis.Model.Common;
using System;

namespace Axis.Model.ECI
{
    public class NoteHeaderModel : BaseEntity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="NoteHeaderModel"/> class.
        /// </summary>
        public NoteHeaderModel()
        {
            Discharge = new DischargeModel();
            ProgressNote = new ProgressNoteModel();
            ProgressNoteAssessment = new ProgressNoteAssessmentModel();
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
        public DischargeModel Discharge { get; set; }

        /// <summary>
        /// Gets or sets the progress note.
        /// </summary>
        /// <value>
        /// The progress note.
        /// </value>
        public ProgressNoteModel ProgressNote { get; set; }

        /// <summary>
        /// Gets or sets the progress note assessment.
        /// </summary>
        /// <value>
        /// The progress note assessment.
        /// </value>
        public ProgressNoteAssessmentModel ProgressNoteAssessment { get; set; }
    }
}
