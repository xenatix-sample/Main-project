namespace Axis.Model.Common
{
    /// <summary>
    /// Represents payor details
    /// </summary>
    public class PayorModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the payor identifier.
        /// </summary>
        /// <value>
        /// The payor identifier.
        /// </value>
        public int PayorID { get; set; }

        /// <summary>
        /// Gets or sets the name of the payor.
        /// </summary>
        /// <value>
        /// The name of the payor.
        /// </value>
        public string PayorName { get; set; }


        /// <summary>
        /// Gets or sets the payor code.
        /// </summary>
        /// <value>
        /// The Code of the payor.
        /// </value>
        public int PayorCode { get; set; }
    }
}