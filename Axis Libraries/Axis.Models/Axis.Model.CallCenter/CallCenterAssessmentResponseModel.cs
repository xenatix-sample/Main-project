using Axis.Model.Common;

namespace Axis.Model.CallCenter
{
    /// <summary>
    ///
    /// </summary>
    public class CallCenterAssessmentResponseModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the call center assessment response identifier.
        /// </summary>
        /// <value>
        /// The call center assessment response identifier.
        /// </value>
        public long CallCenterAssessmentResponseID { get; set; }

        /// <summary>
        /// Gets or sets the call center header identifier.
        /// </summary>
        /// <value>
        /// The call center header identifier.
        /// </value>
        public long CallCenterHeaderID { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public long ResponseID { get; set; }
    }
}