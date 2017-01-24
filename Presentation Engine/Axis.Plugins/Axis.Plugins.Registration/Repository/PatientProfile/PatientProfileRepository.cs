using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Model;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Globalization;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository.PatientProfile
{
    public class PatientProfileRepository : IPatientProfileRepository
    {
        #region Private Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "PatientProfile/";
        private const string getPatientProfile = "GetPatientProfile";

        #endregion Private Variables

        #region Constructors

        public PatientProfileRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public PatientProfileRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }


        #endregion

        #region Public Methods

        /// <summary>
        /// Gets the Patient Profile.
        /// </summary>
        /// <param name="contactID">The contact identifier.</param>
        /// <returns></returns>
       
        public async Task<Response<PatientProfileViewModel>> GetPatientProfile(long contactID)
        {
            string apiUrl = string.Format("{0}{1}", BaseRoute, getPatientProfile);
            var parameters = new NameValueCollection { { "contactID", contactID.ToString(CultureInfo.InvariantCulture) } };
            var response = await _communicationManager.GetAsync<Response<PatientProfileModel>>(parameters, apiUrl);
            return response.ToViewModel();
        }
        #endregion

      
    }
}
