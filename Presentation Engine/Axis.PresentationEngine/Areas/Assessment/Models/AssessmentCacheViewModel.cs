namespace Axis.PresentationEngine.Areas.Assessment.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AssessmentCacheViewModel
    {
        /// <summary>
        /// Gets or sets the assessments JSON data.
        /// </summary>
        /// <value>
        /// The assessments.
        /// </value>
        public string Assessments { get; set; }

        /// <summary>
        /// Gets or sets the assessment sections JSON data.
        /// </summary>
        /// <value>
        /// The assessment sections.
        /// </value>
        public string AssessmentSections { get; set; }

        /// <summary>
        /// Gets or sets the assessment questions JSON data.
        /// </summary>
        /// <value>
        /// The assessment questions.
        /// </value>
        public string AssessmentQuestions { get; set; }

        /// <summary>
        /// Gets or sets the Assessmentlogic questions
        /// </summary>
        public string AssessmentLogicMapping { get; set; }
    }
}