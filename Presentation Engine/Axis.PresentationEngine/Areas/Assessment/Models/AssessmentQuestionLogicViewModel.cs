using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using Axis.Model.Common;


namespace Axis.PresentationEngine.Areas.Assessment.Models
{
    public class AssessmentQuestionLogicViewModel: BaseEntity
    {   
      
            /// <summary>
            /// Set the lOgic Mapping Id
            /// </summary>
            public int? LogicMappingID { get; set; }

            /// <summary>
            /// Set or get the Logic Id
            /// </summary>
            public int? LogicID { get; set; }

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

            /// <summary>
            /// Set or get the Logic code
            /// </summary>
            public string LogicCode { get; set; }

        /// <summary>
        /// Set or get the IsActive status
        /// </summary>
        public new bool? IsActive { get; set; }

        /// <summary>
        /// Set or get the who modified by
        /// </summary>
        public new int? ModifiedBy { get; set; }

        /// <summary>
        /// Set or get the Modified On
        /// </summary>
        public new DateTime? ModifiedOn { get; set; }

        /// <summary>
        /// Set or get the Created By.
        /// </summary>
        public new int? CreatedBy { get; set; }

        /// <summary>
        /// Set or get the  Created On
        /// </summary>
        public new DateTime? CreatedOn { get; set; }

        /// <summary>
        /// List of values.
        /// </summary>

        public List<AssessmentQuestionLogicViewModel> AssessmentLogicQuestionMapping { get; set; }

    }
}