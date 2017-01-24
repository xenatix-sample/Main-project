namespace Axis.PresentationEngine.Helpers.Model
{
    /// <summary>
    /// Represents mail permission
    /// </summary>
    public class MailPermissionViewModel : BaseViewModel
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