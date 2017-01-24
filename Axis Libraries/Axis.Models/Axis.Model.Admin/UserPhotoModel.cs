using Axis.Model.Photo;

namespace Axis.Model.Admin
{
    /// <summary>
    ///
    /// </summary>
    public class UserPhotoModel : PhotoModel
    {
        /// <summary>
        /// Gets or sets the user photo identifier.
        /// </summary>
        /// <value>
        /// The user photo identifier.
        /// </value>
        public long UserPhotoID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int UserID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }
    }
}