using Axis.Model.Common;

namespace Axis.Model.Phone
{
    /// <summary>
    /// Represents Phone
    /// </summary>
    public class PhoneModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the phone identifier.
        /// </summary>
        /// <value>
        /// The phone identifier.
        /// </value>
        public long PhoneID { get; set; }

        /// <summary>
        /// Gets or sets the phone type identifier.
        /// </summary>
        /// <value>
        /// The phone type identifier.
        /// </value>
        public int? PhoneTypeID { get; set; }

        /// <summary>
        /// Gets or sets the number.
        /// </summary>
        /// <value>
        /// The number.
        /// </value>
        public string Number { get; set; }

        /// <summary>
        /// Gets or sets the extension.
        /// </summary>
        /// <value>
        /// The extension.
        /// </value>
        public string Extension { get; set; }
    }
}