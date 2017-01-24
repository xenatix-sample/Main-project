namespace Axis.Model.Common
{
    /// <summary>
    ///
    /// </summary>
    public class TitleModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the title identifier.
        /// </summary>
        /// <value>
        /// The title identifier.
        /// </value>
        public int TitleID { get; set; }

        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        public string Title { get; set; }
    }
}