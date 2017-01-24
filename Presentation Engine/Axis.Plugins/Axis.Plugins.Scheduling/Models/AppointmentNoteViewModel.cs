using Axis.PresentationEngine.Helpers.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using Axis.Model.Scheduling;
using Axis.Plugins.Scheduling.Translator;

namespace Axis.Plugins.Scheduling.Models
{
    public class AppointmentNoteViewModel : BaseViewModel
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
