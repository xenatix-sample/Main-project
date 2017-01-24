using System;

namespace Axis.Model.Registration
{
    /// <summary>
    /// Model for Financial Assessment List
    /// </summary>
    public class FinancialAssessmentListModel
    {
        /// <summary>
        /// Gets or sets the financial assessment identifier.
        /// </summary>
        /// <value>
        /// The financial assessment identifier.
        /// </value>
        public long FinancialAssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the assessment date.
        /// </summary>
        /// <value>
        /// The assessment date.
        /// </value>
        public DateTime AssessmentDate { get; set; }
    }
}