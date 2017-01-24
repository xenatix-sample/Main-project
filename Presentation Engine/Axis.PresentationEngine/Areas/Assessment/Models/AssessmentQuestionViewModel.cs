using Axis.Model.Common;
using System.Collections.Generic;

namespace Axis.PresentationEngine.Areas.Assessment.Models
{
    /// <summary>
    /// Represents Assessement Questions
    /// </summary>
    public class AssessmentQuestionViewModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public long QuestionID { get; set; }

        /// <summary>
        /// Gets or sets the assessment section identifier.
        /// </summary>
        /// <value>
        /// The assessment section identifier.
        /// </value>
        public long? AssessmentSectionID { get; set; }

        /// <summary>
        /// Gets or sets the parent question identifier.
        /// </summary>
        /// <value>
        /// The parent question identifier.
        /// </value>
        public long? ParentQuestionID { get; set; }

        /// <summary>
        /// Gets or sets the parent options identifier.
        /// </summary>
        /// <value>
        /// The parent options identifier.
        /// </value>
        public long? ParentOptionsID { get; set; }

        /// <summary>
        /// Gets or sets the question.
        /// </summary>
        /// <value>
        /// The question.
        /// </value>
        public string Question { get; set; }

        /// <summary>
        /// Gets or sets the question type identifier.
        /// </summary>
        /// <value>
        /// The question type identifier.
        /// </value>
        public int QuestionTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the question.
        /// </summary>
        /// <value>
        /// The type of the question.
        /// </value>
        public string QuestionType { get; set; }

        /// <summary>
        /// Gets or sets the options group identifier.
        /// </summary>
        /// <value>
        /// The options group identifier.
        /// </value>
        public long? OptionsGroupID { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is answer required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is answer required; otherwise, <c>false</c>.
        /// </value>
        public int? IsAnswerRequired { get; set; }

        /// <summary>
        /// Gets or sets the input type position identifier.
        /// </summary>
        /// <value>
        /// The input type position identifier.
        /// </value>
        public int? InputTypePositionID { get; set; }

        /// <summary>
        /// Gets or sets the input type position.
        /// </summary>
        /// <value>
        /// The input type position.
        /// </value>
        public string InputTypePosition { get; set; }

        /// <summary>
        /// Gets or sets the input type identifier.
        /// </summary>
        /// <value>
        /// The input type identifier.
        /// </value>
        public int? InputTypeID { get; set; }

        /// <summary>
        /// Gets or sets the type of the input.
        /// </summary>
        /// <value>
        /// The type of the input.
        /// </value>
        public string InputType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this instance is numbering required.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is numbering required; otherwise, <c>false</c>.
        /// </value>
        public bool? IsNumberingRequired { get; set; }

        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The image identifier.
        /// </value>
        public long? ImageID { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public long? SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the key by which to look this question up (for use in reporting).
        /// </summary>
        /// <value>
        /// The data key.
        /// </value>
        public long DataKey { get; set; }

        /// <summary>
        /// Gets or sets the container attributes.
        /// </summary>
        /// <value>
        /// The container attributes.
        /// </value>
        public string ContainerAttributes { get; set; }

        /// <summary>
        /// Gets or sets the attributes.
        /// </summary>
        /// <value>
        /// The attributes.
        /// </value>
        public string Attributes { get; set; }

        /// <summary>
        /// Gets or sets the assessment question options.
        /// </summary>
        /// <value>
        /// The assessment question options.
        /// </value>
        public List<AssessmentQuestionOptionViewModel> AssessmentQuestionOptions { get; set; }
    }
}