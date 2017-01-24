using Axis.PresentationEngine.Helpers.Model;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class ProviderViewModel : BaseViewModel
    {
        /// <summary>
        /// Gets or sets the provider identifier.
        /// </summary>
        /// <value>
        /// The provider identifier.
        /// </value>
        public long ProviderId { get; set; }

        /// <summary>
        /// Gets or sets the name of the provider.
        /// </summary>
        /// <value>
        /// The name of the provider.
        /// </value>
        public string ProviderName { get; set; }

        /// <summary>
        /// Gets or sets the credential identifier.
        /// </summary>
        /// <value>
        /// The credential identifier.
        /// </value>
        public long CredentialId { get; set; }
    }
}