using System.Collections.Generic;

namespace Axis.Model.Common.Assessment
{
    /// <summary>
    ///
    /// </summary>
    public class AssessmentSectionsModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the assessment section identifier.
        /// </summary>
        /// <value>
        /// The assessment section identifier.
        /// </value>
        public long AssessmentSectionID { get; set; }

        /// <summary>
        /// Gets or sets the name of the section.
        /// </summary>
        /// <value>
        /// The name of the section.
        /// </value>
        public string SectionName { get; set; }

        /// <summary>
        /// Gets or sets the assessment identifier.
        /// </summary>
        /// <value>
        /// The assessment identifier.
        /// </value>
        public long? AssessmentID { get; set; }

        /// <summary>
        /// Gets or sets the sort order.
        /// </summary>
        /// <value>
        /// The sort order.
        /// </value>
        public long? SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the permission key.
        /// </summary>
        /// <value>
        /// The permission key.
        /// </value>
        public string PermissionKey { get; set; }
    }
}