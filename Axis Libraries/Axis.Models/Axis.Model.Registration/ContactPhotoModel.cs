using Axis.Model.Photo;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class ContactPhotoModel : PhotoModel
    {
        /// <summary>
        /// Gets or sets the contact photo identifier.
        /// </summary>
        /// <value>
        /// The contact photo identifier.
        /// </value>
        public long ContactPhotoID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is primary.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is primary; otherwise, <c>false</c>.
        /// </value>
        public bool IsPrimary { get; set; }
    }
}