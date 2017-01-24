using System;

namespace Axis.Plugins.Clinical.Models
{
    public class NoteViewModel : ClinicalBaseViewModel
    {
        /// <summary>
        /// NoteID
        /// </summary>
        public long NoteID { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }
        /// <summary>
        /// NoteTypeID
        /// </summary>
        public short NoteTypeID { get; set; }
        /// <summary>
        /// NoteStatusID
        /// </summary>
        public short? NoteStatusID { get; set; }
        /// <summary>
        /// DocumentStatusID
        /// </summary>
        public short DocumentStatusID { get; set; }
    }
}
