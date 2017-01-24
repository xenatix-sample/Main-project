using Axis.Configuration;
using Axis.Constant;
using Axis.Helpers;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Plugins.Registration.Models;
using Axis.Plugins.Registration.Translator;
using Axis.Service;
using System.Collections.Specialized;
using System.Threading.Tasks;

namespace Axis.Plugins.Registration.Repository
{
    /// <summary>
    ///
    /// </summary>
    public class ConsentRepository : IConsentRepository
    {
        #region Class Variables

        /// <summary>
        /// The _communication manager
        /// </summary>
        private readonly CommunicationManager _communicationManager;

        /// <summary>
        /// The base route
        /// </summary>
        private const string baseRoute = "Consent/";

        #endregion Class Variables

        #region Constructor

        /// <summary>
        /// Constructor
        /// </summary>
        public ConsentRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsentRepository" /> class.
        /// </summary>
        /// <param name="token">The token.</param>
        public ConsentRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion Constructor

        #region Public Methods

        /// <summary>
        /// Adds the consent signature.
        /// </summary>
        /// <param name="consentViewModel">The consent view model.</param>
        /// <returns></returns>
        
        public Response<ConsentViewModel> AddConsentSignature(ConsentViewModel consentViewModel)
        {
            string apiUrl = baseRoute + "AddConsentSignature";
            var response = _communicationManager.Post<ConsentModel, Response<ConsentModel>>(consentViewModel.ToModel(), apiUrl);
            return response.ToModel();
        }

        /// <summary>
        /// Gets the consent signature.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
      
        public Response<ConsentViewModel> GetConsentSignature(long contactId)
        {
            return GetConsentSignatureAsync(contactId).Result;
        }

        /// <summary>
        /// Gets the consent signature.
        /// </summary>
        /// <param name="contactId">The contact identifier.</param>
        /// <returns></returns>
      
        async public Task<Response<ConsentViewModel>> GetConsentSignatureAsync(long contactId)
        {
            string apiUrl = baseRoute + "GetConsentSignature";
            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());

            var response = await _communicationManager.GetAsync<Response<ConsentModel>>(param, apiUrl);
            return response.ToModel();
        }

        #endregion Public Methods
    }
}