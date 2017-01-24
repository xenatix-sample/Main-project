using Axis.PresentationEngine.Helpers.Model;
using System.Collections.Generic;

namespace Axis.Plugins.Scheduling.Models
{
    /// <summary>
    ///
    /// </summary>
    public class AppointmentCredentialViewModel : BaseViewModel
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AppointmentCredentialViewModel"/> class.
        /// </summary>
        public AppointmentCredentialViewModel()
        {
            Providers = new List<ProviderViewModel>();
        }

        /// <summary>
        /// Gets or sets the appointment type identifier.
        /// </summary>
        /// <value>
        /// The appointment type identifier.
        /// </value>
        public int AppointmentTypeID { get; set; }

        /// <summary>
        /// Gets or sets the credential identifier.
        /// </summary>
        /// <value>
        /// The credential identifier.
        /// </value>
        public long CredentialID { get; set; }

        /// <summary>
        /// Gets or sets the credential code.
        /// </summary>
        /// <value>
        /// The credential code.
        /// </value>
        public string CredentialCode { get; set; }

        /// <summary>
        /// Gets or sets the credential abbreviation.
        /// </summary>
        /// <value>
        /// The credential abbreviation.
        /// </value>
        public string CredentialAbbreviation { get; set; }

        /// <summary>
        /// Gets or sets the name of the credential.
        /// </summary>
        /// <value>
        /// The name of the credential.
        /// </value>
        public string CredentialName { get; set; }

        /// <summary>
        /// Gets or sets the providers.
        /// </summary>
        /// <value>
        /// The providers.
        /// </value>
        public List<ProviderViewModel> Providers { get; set; }
    }
}