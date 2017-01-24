using Axis.Model.Common;
using System;

namespace Axis.Model.ECI
{
    public class ProgressNoteAssessmentModel : BaseEntity
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
    }
}
