using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Registration.Model
{
    public class EmailViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the email identifier.
        /// </summary>
        /// <value>
        /// The email identifier.
        /// </value>
        public long EmailID { get; set; }

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; }
    }
}