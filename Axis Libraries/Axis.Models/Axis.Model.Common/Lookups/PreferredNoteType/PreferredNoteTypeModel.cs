using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Common
{
    public class PreferredNoteTypeModel
    {
        /// <summary>
        /// Gets or sets the NoteType identifier.
        /// </summary>
        /// <value>
        /// The NoteType identifier.
        /// </value>
        public int NoteTypeID { get; set; }

        /// <summary>
        /// Gets or sets the name of the NoteType.
        /// </summary>
        /// <value>
        /// The name of the NoteType.
        /// </value>
        public string NoteType { get; set; }
    }
}
