using Axis.Model.Common;

namespace Axis.DataProvider.Common
{
    /// <summary>
    /// Category Type Model
    /// </summary>
    public class FinanceCategoryTypeModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the category type identifier.
        /// </summary>
        /// <value>
        /// The category type identifier.
        /// </value>
        public int CategoryTypeID { get; set; }
        /// <summary>
        /// Gets or sets the type of the category.
        /// </summary>
        /// <value>
        /// The type of the category.
        /// </value>
        public string CategoryType { get; set; }
    }
}