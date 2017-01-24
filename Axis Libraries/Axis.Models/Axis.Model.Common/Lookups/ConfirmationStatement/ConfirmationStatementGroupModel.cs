namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    /// <seealso cref="Axis.Model.Common.BaseEntity" />
    public class ConfirmationStatementGroupModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the confirmation statement group identifier.
        /// </summary>
        /// <value>
        /// The confirmation statement group identifier.
        /// </value>
        public int ConfirmationStatementGroupID { get; set; }

        /// <summary>
        /// Gets or sets the confirmation statement group.
        /// </summary>
        /// <value>
        /// The confirmation statement group.
        /// </value>
        public string ConfirmationStatementGroup { get; set; }
    }
}