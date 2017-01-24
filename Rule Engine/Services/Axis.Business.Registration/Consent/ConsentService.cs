using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Model.Common;
using Axis.Model.Registration;
using Axis.Security;

namespace Axis.Service.Registration.Consent
{
    public class ConsentService : IConsentService
    {
         #region Class Variables

        private CommunicationManager _communicationManager;
        private const string baseRoute = "consent/";

        #endregion

         #region Constructors

        public ConsentService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<ConsentModel> AddConsentSignature(ConsentModel consent)
        {
            var apiUrl = baseRoute + "AddConsentSignature";
            return _communicationManager.Post<ConsentModel, Response<ConsentModel>>(consent, apiUrl);
        }

        public Response<ConsentModel> GetConsentSignature(long contactId)
        {
            var apiUrl = baseRoute + "GetConsentSignature";
            var param = new NameValueCollection();
            param.Add("contactId", contactId.ToString());

            return _communicationManager.Get<Response<ConsentModel>>(param, apiUrl);
        }

        #endregion
    }
}
