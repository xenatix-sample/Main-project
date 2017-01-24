using System;
using System.Collections.Generic;

namespace Axis.Model.Common.Assessment
{
    /// <summary>
    ///
    /// </summary>
    public class AssessmentResponseDetailsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the response detail identifier.
        /// </summary>
        /// <value>
        /// The response detail identifier.
        /// </value>
        public long ResponseDetailsID { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public long ResponseID { get; set; }

        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public long QuestionID { get; set; }

        /// <summary>
        /// Gets or sets the options identifier.
        /// </summary>
        /// <value>
        /// The options identifier.
        /// </value>
        public long? OptionsID { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public string Options { get; set; }

        /// <summary>
        /// Gets or sets the response text.
        /// </summary>
        /// <value>
        /// The response text.
        /// </value>
        public string ResponseText { get; set; }

        /// <summary>
        /// Gets or sets the rating.
        /// </summary>
        public int Rating { get; set; }

        /// <summary>
        /// Gets or sets the signature BLOB.
        /// </summary>
        /// <value>
        /// The signature BLOB.
        /// </value>
        public Byte[] SignatureBLOB { get; set; }

        /// <summary>
        /// Gets or sets the audit XML. To get snapshot of the saved data in XML
        /// </summary>
        /// <value>
        /// The audit XML.
        /// </value>
        public string AuditXML { get; set; }

        /// <summary>
        /// Gets or sets the date signed.
        /// </summary>
        /// <value>
        /// The date signed.
        /// </value>
        public DateTime DateSigned { get; set; }

        /// <summary>
        /// Gets or sets the credential identifier.
        /// </summary>
        /// <value>
        /// The credential identifier.
        /// </value>
        public int CredentialID { get; set; }
    }
}
