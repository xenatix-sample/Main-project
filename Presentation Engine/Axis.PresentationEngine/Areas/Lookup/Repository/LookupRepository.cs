using System.Collections.Generic;
using System.Collections.Specialized;
using Axis.Configuration;
using Axis.Helpers;
using Axis.Service;
using Axis.Model.Common;
using Axis.PresentationEngine.Helpers.Repositories;

namespace Axis.PresentationEngine.Areas.Lookup.Repository
{
    public class LookupRepository : ILookupRepository
    {
        #region Class Variables

        private readonly CommunicationManager _communicationManager;
        private const string BaseRoute = "Lookup/";

        #endregion

        #region Constructors

        public LookupRepository()
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, Cookies.GetCookies(ApplicationSettings.Token));
        }

        public LookupRepository(string token)
        {
            _communicationManager = new CommunicationManager(ApplicationSettings.Token, token);
        }

        #endregion

        #region Methods

        public Response<Dictionary<string, List<dynamic>>> GetLookupsToCache()
        {
            string apiUrl = BaseRoute + "GetLookupsToCache";

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(apiUrl);

            return response;
        }

        public Response<Dictionary<string, List<dynamic>>> GetReportsToCache()
        {
            string apiUrl = BaseRoute + "GetReportsToCache";

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(apiUrl);

            return response;
        }
        
        public Response<Dictionary<string, List<dynamic>>> GetDrugsToCache()
        {
            string apiUrl = BaseRoute + "GetDrugsToCache";

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(apiUrl);

            return response;
        }

        public Response<Dictionary<string, List<dynamic>>> GetLookupsByType(LookupType lookupType)
        {
            string apiUrl = BaseRoute + "GetLookupsByType";

            var parameters = new NameValueCollection { { "lookupType", lookupType.ToString() } };

            var response = _communicationManager.Get<Response<Dictionary<string, List<dynamic>>>>(parameters, apiUrl);

            return response;
        }

        #endregion
    }
}