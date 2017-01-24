namespace Axis.Model.Common
{
    /// <summary>
    /// Represent PolicyHolder
    /// </summary>
    public class PolicyHolderModel
    {
        /// <summary>
        /// Gets or sets the policy holder identifier.
        /// </summary>
        /// <value>
        /// The policy holder identifier.
        /// </value>
        public int PolicyHolderID { get; set; }

        /// <summary>
        /// Gets or sets the policy holder.
        /// </summary>
        /// <value>
        /// The policy holder.
        /// </value>
        public string PolicyHolder { get; set; }
    }
}