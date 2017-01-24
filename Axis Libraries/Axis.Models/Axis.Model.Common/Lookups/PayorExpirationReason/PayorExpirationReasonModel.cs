namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class PayorExpirationReasonModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the payor expiration reason identifier.
        /// </summary>
        /// <value>
        /// The payor expiration reason identifier.
        /// </value>
        public int PayorExpirationReasonID { get; set; }

        /// <summary>
        /// Gets or sets the payor expiration reason.
        /// </summary>
        /// <value>
        /// The payor expiration reason.
        /// </value>
        public string PayorExpirationReason { get; set; }
    }
}