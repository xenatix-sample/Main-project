using Axis.Model.Common;
using Axis.Model.ESignature;
using System;

namespace Axis.Model.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class FinancialSummaryConfirmationStatementModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the financial summary confirmation statement identifier.
        /// </summary>
        /// <value>
        /// The financial summary confirmation statement identifier.
        /// </value>
        public long FinancialSummaryConfirmationStatementID { get; set; }

        /// <summary>
        /// Gets or sets the financial summary identifier.
        /// </summary>
        /// <value>
        /// The financial summary identifier.
        /// </value>
        public long FinancialSummaryID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

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
        /// Gets or sets the date signed.
        /// </summary>
        /// <value>
        /// The date signed.
        /// </value>
        public DateTime? DateSigned { get; set; }

        /// <summary>
        /// Gets or sets the signature status identifier.
        /// </summary>
        /// <value>
        /// The signature status identifier.
        /// </value>
        public int SignatureStatusID { get; set; }

        /// <summary>
        /// Gets or sets the entity signature identifier.
        /// </summary>
        /// <value>
        /// The entity signature identifier.
        /// </value>
        public long? EntitySignatureID { get; set; }

        /// <summary>
        /// Gets or sets the signature BLOB.
        /// </summary>
        /// <value>
        /// The signature BLOB.
        /// </value>
        public byte[] SignatureBLOB { get; set; }
    }
}