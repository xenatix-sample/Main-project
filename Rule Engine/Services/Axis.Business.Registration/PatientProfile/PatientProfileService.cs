using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;
using System.Collections.Specialized;
using System.Globalization;

namespace Axis.Service.Registration
{
    /// <summary>
    ///
    /// </summary>
    public class PatientProfileService : IPatientProfileService
    {
        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string BaseRoute = "PatientProfile/";

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationService"/> class.
        /// </summary>
        public PatientProfileService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        /// <summary>
        /// Gets the Patient Profile
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
        public Response<PatientProfileModel> GetPatientProfile(long contactID)
        {
            string apiUrl = BaseRoute + "GetPatientProfile";
            var requestXMLValueNvc = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };

            return _communicationManager.Get<Response<PatientProfileModel>>(requestXMLValueNvc, apiUrl);
        }

        
    }
}