using System.Collections.Generic;

namespace Axis.Model.Common.Assessment
{
    /// <summary>
    /// Represents Assessment
    /// </summary>
    public class AssessmentModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the name of the assessment.
        /// </summary>
        /// <value>
        /// The name of the assessment.
        /// </value>
        public string AssessmentName { get; set; }

        /// <summary>
        /// Gets or sets the version identifier.
        /// </summary>
        /// <value>
        /// The version identifier.
        /// </value>
        public string VersionID { get; set; }

        /// <summary>
        /// Gets or sets the frequency identifier.
        /// </summary>
        /// <value>
        /// The frequency identifier.
        /// </value>
        public int? FrequencyID { get; set; }

        /// <summary>
        /// Gets or sets the frequency.
        /// </summary>
        /// <value>
        /// The frequency.
        /// </value>
        public string Frequency { get; set; }

        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int? CategoryID { get; set; }

        /// <summary>
        /// Gets or sets the name of the category.
        /// </summary>
        /// <value>
        /// The name of the category.
        /// </value>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the category abbreviation.
        /// </summary>
        /// <value>
        /// The category abbreviation.
        /// </value>
        public string CategoryAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets the program identifier.
        /// </summary>
        /// <value>
        /// The program identifier.
        /// </value>
        public int? ProgramID { get; set; }

        /// <summary>
        /// Gets or sets the name of the program.
        /// </summary>
        /// <value>
        /// The name of the program.
        /// </value>
        public string ProgramName { get; set; }

        /// <summary>
        /// Gets or sets the image identifier.
        /// </summary>
        /// <value>
        /// The image identifier.
        /// </value>
        public long? ImageID { get; set; }

        /// <summary>
        /// Gets or sets the content of the image.
        /// </summary>
        /// <value>
        /// The content of the image.
        /// </value>
        public byte[] ImageContent { get; set; }

        /// <summary>
        /// Gets or sets the assessment sections.
        /// </summary>
        /// <value>
        /// The assessment sections.
        /// </value>
        public List<AssessmentSectionsModel> AssessmentSections { get; set; }

        /// <summary>
        /// Gets Set DocumentTypeID
        /// </summary>
        public int? DocumentTypeID { get; set; }
    }
}