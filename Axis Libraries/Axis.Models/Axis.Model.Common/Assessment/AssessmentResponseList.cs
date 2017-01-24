using System;
using Axis.Model.Common;

namespace Axis.Model.Common.Assessment
{
    public class AssessmentResponseList : BaseEntity
    {
        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the Assessment Name.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public string AssessmentName { get; set; }
       
        /// <summary>
        /// Gets or sets the assessment response identifier.
        /// </summary>
        /// <value>
        /// The assessment response identifier.
        /// </value>
        public long ResponseID { get; set; }


        /// <summary>
        /// Gets or sets the call center header identifier.
        /// </summary>
        /// <value>
        /// The call center header identifier.
        /// </value>
        public long CallCenterHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the source.
        /// </summary>
        /// <value>
        /// The source.
        /// </value>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets the service start date.
        /// </summary>
        /// <value>
        /// The service start date.
        /// </value>
        public DateTime? ServiceStartDate { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public long ProviderID { get; set; }

        /// <summary>
        /// Gets or sets the service recording id.
        /// </summary>
        /// <value>
        /// The service recording indentifier.
        /// </value>
        public long? ServiceRecordingID { get; set; }

        /// <summary>
        /// Gets or sets the organization identifier.
        /// </summary>
        /// <value>
        /// The organization identifier.
        /// </value>
        public long? OrganizationID { get; set; }

        /// <summary>
        /// Gets or sets the call status.
        /// </summary>
        /// <value>
        /// The call status
        /// </value>
        public string CallStatus { get; set; }

        /// <summary>
        /// Gets or sets the service recording source identifier.
        /// </summary>
        /// <value>
        /// The service recording source identifier.
        /// </value>
        public int? ServiceRecordingSourceID { get; set; }
    }
}
