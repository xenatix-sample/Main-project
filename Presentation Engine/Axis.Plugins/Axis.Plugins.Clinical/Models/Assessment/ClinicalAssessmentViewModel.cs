using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Axis.Plugins.Clinical.Models.Assessment
{
    public class ClinicalAssessmentViewModel : ClinicalBaseViewModel
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
