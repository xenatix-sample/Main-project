using System;

namespace Axis.PresentationEngine.Helpers.Model
{
    public class PhotoViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the photo identifier.
        /// </summary>
        /// <value>
        /// The photo identifier.
        /// </value>
        public long PhotoID { get; set; }

        /// <summary>
        /// Gets or sets the photo BLOB.
        /// </summary>
        /// <value>
        /// The photo BLOB.
        /// </value>
        public byte[] PhotoBLOB { get; set; }

        /// <summary>
        /// Gets or sets the thumbnail BLOB.
        /// </summary>
        /// <value>
        /// The thumbnail BLOB.
        /// </value>
        public byte[] ThumbnailBLOB { get; set; }

        /// <summary>
        /// Gets or sets the taken by.
        /// </summary>
        /// <value>
        /// The taken by.
        /// </value>
        public int TakenBy { get; set; }

        /// <summary>
        /// Gets or sets the taken time.
        /// </summary>
        /// <value>
        /// The taken time.
        /// </value>
        public DateTime TakenTime { get; set; }
    }
}