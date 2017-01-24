using System;
using System.Collections.Generic;

namespace Axis.Model.Common.Assessment
{
    /// <summary>
    ///
    /// </summary>
    public class AssessmentResponseModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public long ResponseID { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the contact identifier.
        /// </summary>
        /// <value>
        /// The contact identifier.
        /// </value>
        public long? ContactID { get; set; }

        /// <summary>
        /// Gets or sets the enter date.
        /// </summary>
        /// <value>
        /// The enter date.
        /// </value>
        public DateTime EnterDate { get; set; }
    }
}