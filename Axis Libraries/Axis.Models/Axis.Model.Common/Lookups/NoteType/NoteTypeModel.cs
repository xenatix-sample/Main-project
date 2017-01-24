
namespace Axis.Model.Common
{
    /// <summary>
    /// Represents NoteType details
    /// </summary>
    public class NoteTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the NoteType identifier.
        /// </summary>
        /// <value>
        /// The NoteType identifier.
        /// </value>
        public short NoteTypeID { get; set; }

        /// <summary>
        /// Gets or sets the name of the NoteType.
        /// </summary>
        /// <value>
        /// The name of the NoteType.
        /// </value>
        public string NoteType { get; set; }
    }
}
