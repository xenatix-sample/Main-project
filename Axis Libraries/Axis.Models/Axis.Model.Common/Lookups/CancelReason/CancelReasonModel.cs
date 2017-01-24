namespace Axis.Model.Common
{
    /// <summary>
    /// Cancel Reason modal
    /// </summary>
    public class CancelReasonModel : BaseEntity
    {
        /// <summary>
        /// identity reasonID
        /// </summary>
        public int ReasonID { get; set; }

        /// <summary>
        /// Reason
        /// </summary>
        public string Reason { get; set; }
    }
}
