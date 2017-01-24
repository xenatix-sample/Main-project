
namespace Axis.Model.Common
{
    /// <summary>
    /// Represents NoteStatus details
    /// </summary>
    public class NoteStatusModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the NoteStatus identifier.
        /// </summary>
        /// <value>
        /// The NoteStatus identifier.
        /// </value>
        public short DocumentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the name of the NoteStatus.
        /// </summary>
        /// <value>
        /// The name of the NoteStatus.
        /// </value>
        public string DocumentStatus { get; set; }
    }
}
