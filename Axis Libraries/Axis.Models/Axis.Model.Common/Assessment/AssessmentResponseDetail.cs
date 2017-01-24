using System;
using System.Collections.Generic;

namespace Axis.Model.Common.Assessment
{
    public class AssessmentResponseDetail : BaseEntity
    {
        /// <summary>
        /// Gets or sets the response identifier.
        /// </summary>
        /// <value>
        /// The response identifier.
        /// </value>
        public long ResponseID { get; set; }

        /// <summary>
        /// Gets or sets the section identifier.
        /// </summary>
        /// <value>
        /// The section identifier.
        /// </value>
        public long SectionID { get; set; }

        /// <summary>
        /// Gets or sets the assessment details.
        /// </summary>
        /// <value>
        /// The assessment details.
        /// </value>
        public List<AssessmentResponseDetailsModel> AssessmentDetails { get; set; }

    }
}
