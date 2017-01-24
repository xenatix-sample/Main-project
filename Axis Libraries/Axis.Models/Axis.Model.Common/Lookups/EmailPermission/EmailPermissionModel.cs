namespace Axis.Model.Common
{
    /// <summary>
    /// Used for email permission's data
    /// </summary>
    public class EmailPermissionModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the email permission identifier.
        /// </summary>
        /// <value>
        /// The email permission identifier.
        /// </value>
        public int EmailPermissionID { get; set; }

        /// <summary>
        /// Gets or sets the email permission.
        /// </summary>
        /// <value>
        /// The email permission.
        /// </value>
        public string EmailPermission { get; set; }
    }
}