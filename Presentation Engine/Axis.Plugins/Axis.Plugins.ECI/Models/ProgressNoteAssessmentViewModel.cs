using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.ECI
{
    public class ProgressNoteAssessmentViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the schedule note assessment identifier.
        /// </summary>
        /// <value>
        /// The schedule note assessment identifier.
        /// </value>
        public long ScheduleNoteAssessmentID { get; set; }
        /// <summary>
        /// Gets or sets the progress note identifier.
        /// </summary>
        /// <value>
        /// The progress note identifier.
        /// </value>
        public long ProgressNoteID { get; set; }
        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime? NoteAssessmentDate { get; set; }
        /// <summary>
        /// Gets or sets the time.
        /// </summary>
        /// <value>
        /// The time.
        /// </value>
        public TimeSpan? NoteAssessmentTime { get; set; }
        /// <summary>
        /// Gets or sets the location identifier.
        /// </summary>
        /// <value>
        /// The location identifier.
        /// </value>
        public int? LocationID { get; set; }
        /// <summary>
        /// Gets or sets the location
        /// </summary>
        /// <value>
        /// The location
        /// </value>
        public string Location { get; set; }
        /// <summary>
        /// Gets or sets the provider identifier.
        /// </summary>
        /// <value>
        /// The provider identifier.
        /// </value>
        public int? ProviderID { get; set; }
        /// <summary>
        /// Gets or sets the members invited.
        /// </summary>
        /// <value>
        /// The members invited.
        /// </value>
        public string MembersInvited { get; set; }

        /// <summary>
        /// Gets or sets the note assessment time secs.
        /// </summary>
        /// <value>
        /// The note assessment time secs.
        /// </value>
        public int NoteAssessmentTimeSecs { get; set; }
    }
}
