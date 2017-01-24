using System;

namespace Axis.PresentationEngine.Helpers.Model
{
    /// <summary>
    /// Represents Client Audit
    /// </summary>
    public class ClientAuditViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the Client ViewCode identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long AuditClientViewCodeID { get; set; }

        /// <summary>
        /// Gets or sets the value that is viewed.
        /// </summary>
        /// <value>
        /// The viewd value.
        /// </value>
        public string ViewedValue { get; set; }

        /// <summary>
        /// Gets or sets any additional data.
        /// </summary>
        /// <value>
        /// Additional Data.
        /// </value>
        public string AdditionalData { get; set; }

        /// <summary>
        /// Gets or sets the Reason.
        /// </summary>
        /// <value>
        /// Reason Text
        /// </value>
        public string ReasonText { get; set; }

        /// <summary>
        /// Gets or sets the date audite created.
        /// </summary>
        /// <value>
        /// Created on Date
        /// </value>
        public new DateTime CreatedOn { get; set; }

        /// <summary>
        /// Gets or sets the created By.
        /// </summary>
        /// <value>
        /// Created By
        /// </value>
        public new string CreatedBy { get; set; }
    }
}
