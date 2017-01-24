namespace Axis.Model.Common
{
    /// <summary>
    /// Represents phone type
    /// </summary>
    public class PhoneTypeModel
    {
        /// <summary>
        /// Gets or sets the phone type identifier.
        /// </summary>
        /// <value>
        /// The phone type identifier.
        /// </value>
        public int PhoneTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the phone.
        /// </summary>
        /// <value>
        /// The type of the phone.
        /// </value>
        public string PhoneType { get; set; }
    }
}