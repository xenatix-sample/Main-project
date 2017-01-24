using Axis.Model.Common;
using System;

namespace Axis.Model.Clinical
{
    /// <summary>
    /// Model for Note
    /// </summary>
    public class NoteModel : ClinicalBaseModel
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
