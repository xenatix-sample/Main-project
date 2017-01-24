namespace Axis.Model.Common
{
    /// <summary>
    /// Represents mail permission
    /// </summary>
    public class MailPermissionModel : BaseEntity
    {
        /// <summary>
        /// Gets or sets the mail permission identifier.
        /// </summary>
        /// <value>
        /// The mail permission identifier.
        /// </value>
        public int MailPermissionID { get; set; }

        /// <summary>
        /// Gets or sets the mail permission.
        /// </summary>
        /// <value>
        /// The mail permission.
        /// </value>
        public string MailPermission { get; set; }
    }
}