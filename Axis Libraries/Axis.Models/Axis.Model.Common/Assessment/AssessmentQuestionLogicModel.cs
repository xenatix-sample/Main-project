namespace Axis.Model.Common.Assessment
{
    public class AssessmentQuestionLogicModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the question identifier.
        /// </summary>
        /// <value>
        /// The question identifier.
        /// </value>
        public long? QuestionID { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long? AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the assessment section identifier.
        /// </summary>
        /// <value>
        /// The assessment section identifier.
        /// </value>
        public long? AssessmentSectionID { get; set; }

        /// <summary>
        /// Set the lOgic Mapping Id
        /// </summary>
        public int? LogicMappingID { get; set; }

        /// <summary>
        /// Set or get the Logic Id
        /// </summary>
        public int? LogicID { get; set; }

        /// <summary>
        /// Set or get the Logic code
        /// </summary>
        public string LogicCode { get; set; }

        /// <summary>
        /// Set or get the question Logic data key
        /// </summary>
        public long? QuestionDataKey { get; set; }

        /// <summary>
        /// Set or get the Logic location Id
        /// </summary>
        public int? LogicLocationId { get; set; }

        /// <summary>
        /// Set or get the LogicOrder
        /// </summary>
        public int? LogicOrder { get; set; }
    }
}