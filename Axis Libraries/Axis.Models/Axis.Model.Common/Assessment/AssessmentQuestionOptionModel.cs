namespace Axis.Model.Common.Assessment
{
    /// <summary>
    /// Represents Assessment Options
    /// </summary>
    public class AssessmentQuestionOptionModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the options identifier.
        /// </summary>
        /// <value>
        /// The options identifier.
        /// </value>
        public long? OptionsID { get; set; }

        /// <summary>
        /// Gets or sets the options.
        /// </summary>
        /// <value>
        /// The options.
        /// </value>
        public string Options { get; set; }

        /// <summary>
        /// Gets or sets the options group identifier.
        /// </summary>
        /// <value>
        /// The options group identifier.
        /// </value>
        public long? OptionsGroupID { get; set; }

        /// <summary>
        /// Gets or sets the name of the options group.
        /// </summary>
        /// <value>
        /// The name of the options group.
        /// </value>
        public string OptionsGroupName { get; set; }

        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public long QuestionID { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public string Attributes { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public long SortOrder { get; set; }
    }
}