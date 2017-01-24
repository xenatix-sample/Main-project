using Axis.PresentationEngine.Helpers.Model;
using System;

namespace Axis.Plugins.ECI.Models
{
    public class ProgressNoteViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the progress note identifier.
        /// </summary>
        /// <value>
        /// The progress note identifier.
        /// </value>
        public long ProgressNoteID { get; set; }

        /// <summary>
        /// Gets or sets the note header identifier.
        /// </summary>
        /// <value>
        /// The note header identifier.
        /// </value>
        public long? NoteHeaderID { get; set; }
        /// <summary>
        /// Gets or sets the start time.
        /// </summary>
        /// <value>
        /// The start time.
        /// </value>
        public TimeSpan? StartTime { get; set; }
        /// <summary>
        /// Gets or sets the end time.
        /// </summary>
        /// <value>
        /// The end time.
        /// </value>
        public TimeSpan? EndTime { get; set; }
        /// <summary>
        /// Gets or sets the contact method identifier.
        /// </summary>
        /// <value>
        /// The contact method identifier.
        /// </value>
        public int? ContactMethodID { get; set; }
        /// <summary>
        /// Gets or sets the contact method other.
        /// </summary>
        /// <value>
        /// The contact method other.
        /// </value>
        public string ContactMethodOther { get; set; }
        /// <summary>
        /// Gets or sets the first name.
        /// </summary>
        /// <value>
        /// The first name.
        /// </value>
        public string FirstName { get; set; }
        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; }
        /// <summary>
        /// Gets or sets the relationship type identifier.
        /// </summary>
        /// <value>
        /// The relationship type identifier.
        /// </value>
        public int? RelationshipTypeID { get; set; }
        /// <summary>
        /// Gets or sets the summary.
        /// </summary>
        /// <value>
        /// The summary.
        /// </value>
        public string Summary { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [reviewed source concerns].
        /// </summary>
        /// <value>
        /// <c>true</c> if [reviewed source concerns]; otherwise, <c>false</c>.
        /// </value>
        public bool ReviewedSourceConcerns { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [reviewed eci process].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reviewed eci process]; otherwise, <c>false</c>.
        /// </value>
        public bool ReviewedECIProcess { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [reviewed eci services].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [reviewed eci services]; otherwise, <c>false</c>.
        /// </value>
        public bool ReviewedECIServices { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [reviewed eci requirements].
        /// </summary>
        /// <value>
        /// <c>true</c> if [reviewed eci requirements]; otherwise, <c>false</c>.
        /// </value>
        public bool ReviewedECIRequirements { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is surrogate parent needed.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is surrogate parent needed; otherwise, <c>false</c>.
        /// </value>
        public bool IsSurrogateParentNeeded { get; set; }
        /// <summary>
        /// Gets or sets the comments.
        /// </summary>
        /// <value>
        /// The comments.
        /// </value>
        public string Comments { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether this instance is discharged.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is discharged; otherwise, <c>false</c>.
        /// </value>
        public bool IsDischarged { get; set; }
        /// <summary>
        /// Gets or sets the progress note date.
        /// </summary>
        /// <value>
        /// The progress note date.
        /// </value>
        public DateTime? ProgressNoteDate { get; set; }
        /// <summary>
        /// Gets or sets the taken by.
        /// </summary>
        /// <value>
        /// The taken by.
        /// </value>
        public int? TakenBy { get; set; }
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
        /// Gets or sets the start time secs.
        /// </summary>
        /// <value>
        /// The start time secs.
        /// </value>
        public int StartTimeSecs { get; set; }
        /// <summary>
        /// Gets or sets the end time secs.
        /// </summary>
        /// <value>
        /// The end time secs.
        /// </value>
        public int EndTimeSecs { get; set; }
    }
}
