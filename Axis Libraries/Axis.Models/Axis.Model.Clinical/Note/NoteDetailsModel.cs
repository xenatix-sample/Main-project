using Axis.Model.Common;
using System;

namespace Axis.Model.Clinical
{
    public class NoteDetailsModel : BaseEntity
    {
        /// <summary>
        /// NoteID
        /// </summary>
        public long NoteID { get; set; }
        /// <summary>
        /// Notes
        /// </summary>
        public string Notes { get; set; }
    }
}
