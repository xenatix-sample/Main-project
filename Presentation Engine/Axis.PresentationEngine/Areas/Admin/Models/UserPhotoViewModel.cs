using Axis.PresentationEngine.Helpers.Model;

namespace Axis.PresentationEngine.Areas.Admin.Models
{
    public class UserPhotoViewModel : PhotoViewModel
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