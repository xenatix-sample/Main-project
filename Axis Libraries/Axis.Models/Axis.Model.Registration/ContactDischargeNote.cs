using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    public class ContactDischargeNote : BaseEntity
    {
        /// <summary>
        /// Contact id
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Contact discharge note id
        /// </summary>
        /// <value>
        /// The contact discharge note identifier.
        /// </value>
        public long ContactDischargeNoteID { get; set; }

        /// <summary>
        /// Note type id
        /// </summary>
        /// <value>
        /// The note type identifier.
        /// </value>
        public long NoteTypeID { get; set; }

        /// <summary>
        /// Contact admission id
        /// </summary>
        /// <value>
        /// The contact admission identifier.
        /// </value>
        public long? ContactAdmissionID { get; set; }


        /// <summary>
        /// Discharge reason id
        /// </summary>
        /// <value>
        /// The discharge reason identifier.
        /// </value>
        public Int32 DischargeReasonID { get; set; }

        /// <summary>
        /// Discharge date
        /// </summary>
        /// <value>
        /// The discharge date.
        /// </value>
        public DateTime DischargeDate { get; set; }

        /// <summary>
        /// Note text
        /// </summary>
        /// <value>
        /// The note text.
        /// </value>
        public string NoteText { get; set; }

        /// <summary>
        /// Gets or sets the signature status identifier.
        /// </summary>
        /// <value>
        /// The signature status identifier.
        /// </value>
        public int? SignatureStatusID { get; set; }

        /// <summary>
        /// Contact Deceased
        /// </summary>
        public bool? IsDeceased { get; set; }
        /// <summary>
        /// Gets or sets the DeceasedDate.
        /// </summary>
        /// <value>
        /// The DeceasedDate.
        /// </value>
        public DateTime? DeceasedDate { get; set; }
    }
}
