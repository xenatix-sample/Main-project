
namespace Axis.Model.Clinical.Assessment
{
    public class ClinicalAssessmentModel : ClinicalBaseModel
    {
        /// <summary>
        /// Clinical Assessment ID
        /// </summary>
        public long ClinicalAssessmentID { get; set; }
        /// <summary>
        /// Assessment ID
        /// </summary>
        public long AssessmentID { get; set; }

        /// <summary>
        /// Response ID
        /// </summary>
        public long? ResponseID { get; set; }

        public long? SectionID { get; set; }
    }
}
