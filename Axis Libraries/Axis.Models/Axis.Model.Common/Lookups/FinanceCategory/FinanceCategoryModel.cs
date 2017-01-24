namespace Axis.Model.Common
{
    /// <summary>
    /// Model to Category
    /// </summary>
    public class FinanceCategoryModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the category identifier.
        /// </summary>
        /// <value>
        /// The category identifier.
        /// </value>
        public int CategoryID { get; set; }
        /// <summary>
        /// Gets or sets the category.
        /// </summary>
        /// <value>
        /// The category.
        /// </value>
        public string Category { get; set; }
        /// <summary>
        /// Gets or sets the category type identifier.
        /// </summary>
        /// <value>
        /// The category type identifier.
        /// </value>
        public int CategoryTypeID { get; set; }
    }
}
