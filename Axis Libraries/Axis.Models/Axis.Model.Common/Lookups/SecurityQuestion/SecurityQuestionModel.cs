namespace Axis.Model.Common.Lookups.SecurityQuestion
{
    /// <summary>
    ///
    /// </summary>
    public class SecurityQuestionModel : BaseEntity
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