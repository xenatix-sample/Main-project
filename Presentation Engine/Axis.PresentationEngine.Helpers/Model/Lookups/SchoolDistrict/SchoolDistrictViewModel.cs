using Axis.Model.Common;

namespace Axis.PresentationEngine.Helpers.Model
{
    /// <summary>
    /// Represents school district
    /// </summary>
    public class SchoolDistrictViewModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the school district identifier.
        /// </summary>
        /// <value>
        /// The school district identifier.
        /// </value>
        public int SchoolDistrictID { get; set; }

        /// <summary>
        /// Gets or sets the county identifier.
        /// </summary>
        /// <value>
        /// The county identifier.
        /// </value>
        public int CountyID { get; set; }

        /// <summary>
        /// Gets or sets the name of the school district.
        /// </summary>
        /// <value>
        /// The name of the school district.
        /// </value>
        public string SchoolDistrictName { get; set; }
    }
}