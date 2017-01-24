using Axis.Model.Common;
using System;
namespace Axis.Plugins.Registration.Models
{
    public class FormsViewModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the contact letters identifier.
        /// </summary>
        /// <value>The contact letters identifier.</value>
        public long ContactFormsID { get; set; }

        /// <summary>
        /// Gets or sets the form status.
        /// </summary>
        /// <value>The form status.</value>
        public short? DocumentStatusID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>The contact identifier.</value>
        public long ContactID { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>The assessment identifier.</value>
        public long? AssessmentID { get; set; }

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
        /// Gets or sets the is voided.
        /// </summary>
        /// <value>
        /// The is voided.
        /// </value>
        public bool? IsVoided { get; set; }

        /// <summary>
        /// Gets or sets the rovider name.
        /// </summary>
        /// <value>The provider name.</value>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>The response identifier.</value>
        public long? ResponseID { get; set; }

        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        public int? UserID { get; set; }

        public DateTime? ServiceEndDate { get; set; }

        public DateTime? ServiceStartDate { get; set; }
    }
}
