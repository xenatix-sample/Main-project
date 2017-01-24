namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    public class ConfirmationStatementModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the confirmation statement identifier.
        /// </summary>
        /// <value>
        /// The confirmation statement identifier.
        /// </value>
        public int ConfirmationStatementID { get; set; }

        /// <summary>
        /// Gets or sets the confirmation statement.
        /// </summary>
        /// <value>
        /// The confirmation statement.
        /// </value>
        public string ConfirmationStatement { get; set; }

        /// <summary>
        /// Gets or sets the confirmation statement group identifier.
        /// </summary>
        /// <value>
        /// The confirmation statement group identifier.
        /// </value>
        public int ConfirmationStatementGroupID { get; set; }

        /// <summary>
        /// Gets or sets the document type identifier.
        /// </summary>
        /// <value>
        /// The document type identifier.
        /// </value>
        public int DocumentTypeID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is signature required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is signature required; otherwise, <c>false</c>.
        /// </value>
        public bool IsSignatureRequired { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public long OrganizationID { get; set; }
    }
}