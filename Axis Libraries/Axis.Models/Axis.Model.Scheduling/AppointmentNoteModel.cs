using Axis.Model.Common;
using System;

namespace Axis.Model.Scheduling
{
    /// <summary>
    /// Model for appointment
    /// </summary>
    public class AppointmentNoteModel : BaseEntity
    {
        /// <summary>
        /// Appointment Note Id
        /// </summary>
        public long AppointmentNoteID { get; set; }

        /// <summary>
        /// Gets or sets the resource id.
        /// </summary>
        public long ContactID { get; set; }

        /// <summary>
        /// Note Type Id
        /// </summary>
        public int NoteTypeID { get; set; }

        /// <summary>
        /// Group Id
        /// </summary>
        public long GroupID { get; set; }

        /// <summary>
        /// User Id
        /// </summary>
        public long UserID { get; set; }

        /// <summary>
        /// Appt Id
        /// </summary>
        public long AppointmentID { get; set; }

        /// <summary>
        /// Note text
        /// </summary>
        public string NoteText { get; set; }
        
    }
}