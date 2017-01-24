using Axis.Model.Common;
using System;

namespace Axis.Model.Registration
{
    public class BenefitsAssistanceModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the benefits assistance identifier.
        /// </summary>
        /// <value>
        /// The benefits assistance identifier.
        /// </value>
        public long BenefitsAssistanceID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the service recording identifier.
        /// </summary>
        /// <value>
        /// The service recording identifier.
        /// </value>
        public long? ServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the Service Recording void identifier.
        /// </summary>
        /// <value>
        /// The Service Recording void identifier.
        /// </value>
        public long? ServiceRecordingVoidID { get; set; }

        /// <summary>
        /// Gets or sets the date entered.
        /// </summary>
        /// <value>
        /// The date entered.
        /// </value>
        public DateTime? DateEntered { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public int? UserID { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long? AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public long? ResponseID { get; set; }

        /// <summary>
        /// Gets or sets the document status identifier.
        /// </summary>
        /// <value>
        /// The document status identifier.
        /// </value>
        public short? DocumentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the is voided.
        /// </summary>
        /// <value>
        /// The is voided.
        /// </value>
        public bool? IsVoided { get; set; }

        /// <summary>
        /// Gets or sets the service start date.
        /// </summary>
        /// <value>
        /// The service start date.
        /// </value>
        public DateTime? ServiceStartDate { get; set; }

        /// <summary>
        /// Gets or sets the service end date.
        /// </summary>
        /// <value>
        /// The service end date.
        /// </value>
        public DateTime?  ServiceEndDate{ get; set; }

        /// <summary>
        /// Gets or sets the service item identifier.
        /// </summary>
        /// <value>
        /// The service item identifier.
        /// </value>
        public int? ServiceItemID{ get; set; }

        /// <summary>
        /// Gets or sets the tracking field identifier.
        /// </summary>
        /// <value>
        /// The tracking field identifier.
        /// </value>
        public int? TrackingFieldID { get; set; }
    }
}
