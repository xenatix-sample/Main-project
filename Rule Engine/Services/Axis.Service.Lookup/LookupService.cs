using Axis.Configuration;
using Axis.Model.Common;
using Axis.Security;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace Axis.Service.Lookup
{
    public class LookupService : ILookupService
    {
        #region Class Variables

        private CommunicationManager _communicationManager;
        private const string BaseRoute = "Lookup/";

        #endregion

        #region Constructors

        public LookupService()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, AuthContext.Auth.Token.Token);
        }

        #endregion

        #region Public Methods

        public Response<Dictionary<string, List<dynamic>>> GetLookupsToCache()
        {
            var apiUrl = BaseRoute + "GetLookupsToCache";

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(apiUrl);
            return response;
        }

        public Response<Dictionary<string, List<dynamic>>> GetReportsToCache()
        {
            var apiUrl = BaseRoute + "GetReportsToCache";

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(apiUrl);
            return response;
        }

        public Response<Dictionary<string, List<dynamic>>> GetDrugsToCache()
        {
            var apiUrl = BaseRoute + "GetDrugsToCache";

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(apiUrl);
            return response;
        }
        
        public Response<Dictionary<string, List<dynamic>>> GetLookupsByType(LookupType lookupType)
        {
            var apiUrl = BaseRoute + "GetLookupsByType";

            var parameters = new NameValueCollection { { "lookupType", lookupType.ToString() } };

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(parameters, apiUrl);
            return response;
        }

        #endregion
    }
}
