using Axis.Model.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Model.Consents
{

    /// <summary>
    /// Consent model
    /// </summary>
    public class ConsentsModel : BaseEntity
    {

        /// <summary>
        /// Get and set ContactConsentID
        /// </summary>
        public long ContactConsentID { get; set; }

        /// <summary>
        /// Get and set AssessmentID
        /// </summary>
        public long AssessmentID { get; set; }

        /// <summary>
        /// Get and set ResponseID
        /// </summary>
        public long ResponseID { get; set; }

        /// <summary>
        /// Get and set DateSigned
        /// </summary>
        public DateTime? DateSigned { get; set; }

        /// <summary>
        ///  Get and set ExpirationDate
        /// </summary>
        public DateTime? ExpirationDate { get; set; }

        /// <summary>
        ///  Get and set ExpirationReasonID
        /// </summary>
        public int? ExpirationReasonID { get; set; }

        /// <summary>
        ///  Get and set ExpirationReason
        /// </summary>
        public string ExpirationReason { get; set; }

        /// <summary>
        ///  Get and set SignatureStatusID
        /// </summary>
        public int SignatureStatusID { get; set; }

        /// <summary>
        ///  Get and set SignatureStatus
        /// </summary>
        public string SignatureStatus { get; set; }

        /// <summary>
        /// Get and set  ConsentName
        /// </summary>
        public string ConsentName { get; set; }

        /// <summary>
        /// Get and set  AssessmentSectionID
        /// </summary>
        public long AssessmentSectionID { get; set; }

        /// <summary>
        /// Get and set  EffectiveDate
        /// </summary>
        public DateTime EffectiveDate { get; set; }

        /// <summary>
        /// Get and set  ConsentName
        /// </summary>
        public DateTime? ExpiredOn { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }

        /// <summary>
        /// Gets or sets the expired response identifier.
        /// </summary>
        /// <value>
        /// The expired response identifier.
        /// </value>
        public long? ExpiredResponseID { get; set; }

        /// <summary>
        /// Gets or sets the expired by.
        /// </summary>
        /// <value>
        /// The expired by.
        /// </value>
        public int? ExpiredBy { get; set; }
    }
}
