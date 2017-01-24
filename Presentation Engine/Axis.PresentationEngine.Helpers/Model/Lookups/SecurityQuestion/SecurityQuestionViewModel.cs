using Axis.Model.Common;

namespace Axis.PresentationEngine.Helpers.Model
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityQuestionViewModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the security question identifier.
        /// </summary>
        /// <value>
        /// The security question identifier.
        /// </value>
        public int SecurityQuestionID { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the question description.
        /// </summary>
        /// <value>
        /// The question description.
        /// </value>
        public string QuestionDescription { get; set; }
    }
}